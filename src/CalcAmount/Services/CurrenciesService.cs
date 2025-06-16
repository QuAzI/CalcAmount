using CalcAmount.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;

namespace CalcAmount.Services
{
    public class CurrenciesService : ICurrenciesService
    {
        private static readonly HttpClient Client = new HttpClient()
        {
            //var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None),

            //ConfigurationSettings.AppSettings.Settings
            //var address = ConfigurationSettings.AppSettings("dsn"),
            //BaseAddress = new Uri("https://api.frankfurter.dev/v1/")
#pragma warning disable CS0618 // Type or member is obsolete
            BaseAddress = new Uri(ConfigurationSettings.AppSettings["ApiEndpoint"])
#pragma warning restore CS0618 // Type or member is obsolete
        };

        public async Task<IReadOnlyDictionary<string, string>> GetCurrenciesAsync()
        {
            using (HttpResponseMessage response = await Client.GetAsync("currencies"))
            {
                response.EnsureSuccessStatusCode();

                var jsonResponse = await response.Content.ReadAsStringAsync();

                var model = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonResponse);

                return model;
                //Directory.CreateDirectory("c:\\temp\\cache");
                //File.WriteAllText(cache, jsonResponse);
            }
        }

        public async Task<CurrenciesResponse> GetRatesFromDate(IReadOnlyList<string> currencies, DateTime startingDate)
        {
            var path = startingDate.ToString("yyyy-MM-dd") + ".." +
                "?symbols=" + string.Join(",", currencies);

            //var cache = $"c:\\temp\\cache\\rates-{path.GetHashCode()}.json";

            //if (!File.Exists(cache))
            {
                using (HttpResponseMessage response = await Client.GetAsync(path))
                {
                    response.EnsureSuccessStatusCode();

                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    
                    var model = JsonConvert.DeserializeObject<CurrenciesResponse>(jsonResponse);

                    //Directory.CreateDirectory("c:\\temp\\cache");
                    //File.WriteAllText(cache, jsonResponse);
                    return model;
                }
            }

            //var data = File.ReadAllText(cache);
            //var model = JsonConvert.DeserializeObject<CurrenciesResponse>(data);

            //return model;
        }
    }
}