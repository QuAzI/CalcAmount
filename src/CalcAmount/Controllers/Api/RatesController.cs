using CalcAmount.Models;
using CalcAmount.Services;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace CalcAmount.Controllers.Api
{
    [RoutePrefix("api/v1.0/rates")]
    public class RatesController : ApiController
    {
        public ICurrenciesService CurrenciesService { get; }

        public readonly string BaseCurrency = "EUR";

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
            var reportingDates = GetReportingDates(DateTime.Now);
            var reportFrom = reportingDates.Last();

            var rates = await CurrenciesService.GetRatesFromDate(request.Currencies, reportFrom);

            var dateReports = new List<DateReportResponse>();
            foreach (var reportingDate in reportingDates)
            {
                var rate = new DateReportResponse
                {
                    Date = reportingDate
                };

                // Note: sometimes there is no rates for 2-3 days so last Monday can be skipped in start of the week
                if (rates.Rates.TryGetValue(reportingDate, out Dictionary<string, double> dateRates))
                {
                    foreach (var specificRate in dateRates)
                    {
                        rate.CurrencyRates.Add(new CurrencyRateResponse
                        {
                            Direction = BaseCurrency + "/" + specificRate.Key,
                            Value = specificRate.Value * request.Amount
                        });
                    }

                    dateReports.Add(rate);
                }
            }

            var response = new RatesResponse
            {
                Directions = request.Currencies.Where(t => !string.IsNullOrEmpty(t)).Select(t => "EUR/" + t).ToArray(),
                Dates = dateReports
            };

            TagExtremeCases(response);

            return Ok(response);
        }

        private IReadOnlyList<DateTime> GetReportingDates(DateTime moment)
        {
            var dates = new List<DateTime>();
            
            var closestMonday = moment.Date;
            while (closestMonday.DayOfWeek != DayOfWeek.Monday)
            {
                closestMonday = closestMonday.AddDays(-1);
            }

            var startingDate = moment.AddDays(-7 * 7);

            var date = closestMonday;
            while (date >= startingDate)
            {
                dates.Add(date);
                date = date.AddDays(-7);
            }

            return dates;
        }

        private void TagExtremeCases(RatesResponse rates)
        {
            var minRates = new Dictionary<string, List<CurrencyRateResponse>>();
            var maxRates = new Dictionary<string, List<CurrencyRateResponse>>();

            foreach (var dateReport in rates.Dates)
            {
                foreach (var rate in dateReport.CurrencyRates)
                {
                    var hasMinValue = minRates.TryGetValue(rate.Direction, out List<CurrencyRateResponse> minRateItems);
                    if (!hasMinValue || (hasMinValue && minRateItems[0].Value > rate.Value))
                    {
                        minRates[rate.Direction] = new List<CurrencyRateResponse> { rate };
                    }
                    else if (minRateItems[0].Value == rate.Value)
                    {
                        minRateItems.Add(rate);
                    }

                    var hasMaxValue = maxRates.TryGetValue(rate.Direction, out List<CurrencyRateResponse> maxRateItems);
                    if (!hasMinValue || (hasMinValue && maxRateItems[0].Value < rate.Value))
                    {
                        maxRates[rate.Direction] = new List<CurrencyRateResponse> { rate };
                    }
                    else if (maxRateItems[0].Value == rate.Value)
                    {
                        maxRateItems.Add(rate);
                    }
                }
            }

            minRates.Values.ForEach(i => i.ForEach(x => x.Tags.Add("min")));
            maxRates.Values.ForEach(i => i.ForEach(x => x.Tags.Add("max")));
        }
    }
}
