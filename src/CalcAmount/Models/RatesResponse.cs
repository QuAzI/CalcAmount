using System.Collections.Generic;

namespace CalcAmount.Models
{
    public class RatesResponse
    {
        public ICollection<RateResponse> Rates { get; set; } = new List<RateResponse>();
        public ICollection<string> Directions { get; internal set; }
    }
}