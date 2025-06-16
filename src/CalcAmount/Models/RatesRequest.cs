using System.Collections.Generic;

namespace CalcAmount.Models
{
    public class RatesRequest
    {
        public float Amount { get; set; }
        public IReadOnlyList<string> Currencies { get; set; }
    }
}