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
        private static readonly HttpClient httpClient = new HttpClient()
        {
            BaseAddress = new Uri("https://api.frankfurter.dev/v1/"),
        };

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

            var rates = await GetRatesFromDate(request.Currencies, startingDate);

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

        private async Task<CurrenciesResponse> GetRatesFromDate(IReadOnlyList<string> currencies, DateTime startingDate)
        {
            var path = startingDate.ToString("yyyy-MM-dd") + ".." +
                "?symbols=" + string.Join(",", currencies);

            var cache = $"c:\\temp\\cache\\rates-{path.GetHashCode()}.json";

            if (!File.Exists(cache))
            {
                using (HttpResponseMessage response = await httpClient.GetAsync(path))
                {
                    response.EnsureSuccessStatusCode();

                    var jsonResponse = await response.Content.ReadAsStringAsync();

                    Directory.CreateDirectory("c:\\temp\\cache");
                    File.WriteAllText(cache, jsonResponse);
                }
            }

            var data = File.ReadAllText(cache);
            var model = JsonConvert.DeserializeObject<CurrenciesResponse>(data);
            
            return model;
        }
    }

    public class RatesRequest
    {
        public float Amount { get; set; }
        public IReadOnlyList<string> Currencies { get; set; }
    }

    public class RatesResponse
    {
        public ICollection<RateResponse> Rates { get; set; } = new List<RateResponse>();
        public ICollection<string> Directions { get; internal set; }
    }

    public class RateResponse
    {
        public DateTime Date { get; set; }
        public ICollection<Rate> Rates { get; set; } = new List<Rate>();
    }

    public class Rate
    {
        public string Direction { get; set; }
        public double Value { get; set; }
    }

    public class CurrenciesResponse
    {
        [JsonProperty("base")]
        public string Base { get; set; }
        [JsonProperty("start_date")]
        public DateTime StartDate { get; set; }
        [JsonProperty("end_date")]
        public DateTime EndDate { get; set; }
        public Dictionary<DateTime, Dictionary<string, double>> Rates { get; set; }
    }
}
