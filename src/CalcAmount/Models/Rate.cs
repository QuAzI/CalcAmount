using System.Collections;
using System.Collections.Generic;

namespace CalcAmount.Models
{
    public class Rate
    {
        public string Direction { get; set; }
        public double Value { get; set; }
        public ICollection<string> Tags { get; set; } = new List<string>();
    }
}