using System;
using System.Collections.Generic;

namespace CalcAmount.Models
{
    public class DateReport
    {
        public DateTime Date { get; set; }
        public ICollection<Rate> CurrencyRates { get; set; } = new List<Rate>();
    }
}