using System;
using System.Collections.Generic;

namespace CalcAmount.Models
{
    public class RateResponse
    {
        public DateTime Date { get; set; }
        public ICollection<Rate> Rates { get; set; } = new List<Rate>();
    }
}