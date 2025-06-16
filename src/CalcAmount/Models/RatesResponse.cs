using System.Collections.Generic;

namespace CalcAmount.Models
{
    public class RatesResponse
    {
        public IReadOnlyCollection<DateReport> Dates { get; set; } = new List<DateReport>();
        public IReadOnlyCollection<string> Directions { get; internal set; }
    }
}