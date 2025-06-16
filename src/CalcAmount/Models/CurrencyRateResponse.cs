using CalcAmount.Helpers;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace CalcAmount.Models
{
    public class CurrencyRateResponse
    {
        public string Direction { get; set; }
        
        [JsonConverter(typeof(FourDecimalDoubleConverter))]
        public double Value { get; set; }
        
        public ICollection<string> Tags { get; set; } = new List<string>();
    }
}