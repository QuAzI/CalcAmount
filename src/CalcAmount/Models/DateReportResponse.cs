using CalcAmount.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace CalcAmount.Models
{
    public class DateReportResponse
    {
        [JsonConverter(typeof(DateOnlyConverter))]
        public DateTime Date { get; set; }

        public ICollection<CurrencyRateResponse> CurrencyRates { get; set; } = new List<CurrencyRateResponse>();
    }
}