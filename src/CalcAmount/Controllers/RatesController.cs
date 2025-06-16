using CalcAmount.Models;
using CalcAmount.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace CalcAmount.Controllers.Api
{
    [RoutePrefix("api/v1.0/rates")]
    public class RatesController : ApiController
    {
        public ICurrenciesService CurrenciesService { get; }

        public RatesController(ICurrenciesService currenciesService)
        {
            CurrenciesService = currenciesService;
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult Get()
        {
            return Ok("Alive");
        }

        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> Post([FromBody] RatesRequest request)
        {
            var response = new RatesResponse
            {
                Directions = request.Currencies.Where(t => !string.IsNullOrEmpty(t)).Select(t => "EUR/" + t).ToArray()
            };

            var closestMonday = DateTime.Now.Date;
            while (closestMonday.DayOfWeek != DayOfWeek.Monday)
            {
                closestMonday = closestMonday.AddDays(-1);
            }
            var startingDate = DateTime.Now.AddDays(-7 * 7);

            var rates = await CurrenciesService.GetRatesFromDate(request.Currencies, startingDate);

            var reportDate = closestMonday;
            while (reportDate >= startingDate)
            {
                var rate = new RateResponse
                {
                    Date = reportDate
                };

                if (rates.Rates.TryGetValue(reportDate, out Dictionary<string, double> dateRates))
                {
                    foreach (var specificRate in dateRates)
                    {
                        rate.Rates.Add(new Rate
                        {
                            Direction = "EUR/" + specificRate.Key,
                            Value = specificRate.Value * request.Amount
                        });
                    }

                    response.Rates.Add(rate);
                }

                reportDate = reportDate.AddDays(-7);
            }

            return Ok(response);
        }
    }
}
