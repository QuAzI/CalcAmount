using CalcAmount.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using System.Runtime.Caching;

namespace CalcAmount.Services
{
    public class CurrenciesService : ICurrenciesService
    {
        private static readonly HttpClient Client = new HttpClient()
        {
#pragma warning disable CS0618 // Type or member is obsolete
            BaseAddress = new Uri(ConfigurationSettings.AppSettings["ApiEndpoint"])
#pragma warning restore CS0618 // Type or member is obsolete
        };

        public async Task<IReadOnlyDictionary<string, string>> GetCurrenciesAsync()
        {
            var memoryCache = MemoryCache.Default;
            var cached = memoryCache.Get("currencies") as IReadOnlyDictionary<string, string>;
            if (cached != null)
            {
                return cached;
            }

            using (HttpResponseMessage response = await Client.GetAsync("currencies"))
            {
                response.EnsureSuccessStatusCode();

                var jsonResponse = await response.Content.ReadAsStringAsync();

                var model = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonResponse);

                memoryCache.Set("currencies", model, DateTime.Now.AddHours(12));

                return model;
            }
        }

        public async Task<CurrenciesReportModel> GetRatesFromDate(IReadOnlyList<string> currencies, DateTime startingDate)
        {
            var key = startingDate.ToString("yyyy-MM-dd") + ".." +
                "?symbols=" + string.Join(",", currencies);

            var memoryCache = MemoryCache.Default;
            var cached = memoryCache.Get(key) as CurrenciesReportModel;
            if (cached != null)
            {
                return cached;
            }

            using (HttpResponseMessage response = await Client.GetAsync(key))
            {
                response.EnsureSuccessStatusCode();

                var jsonResponse = await response.Content.ReadAsStringAsync();

                var model = JsonConvert.DeserializeObject<CurrenciesReportModel>(jsonResponse);

                memoryCache.Set(key, model, DateTime.Now.AddHours(1));

                return model;
            }
        }
    }
}