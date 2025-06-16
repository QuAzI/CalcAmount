using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace CalcAmount.Models
{
    public class CurrenciesReportModel
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