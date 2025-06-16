using System.Collections.Generic;

namespace CalcAmount.Models
{
    public class RatesResponse
    {
        public IReadOnlyCollection<string> Directions { get; internal set; }
        public IReadOnlyCollection<DateReportResponse> Dates { get; set; } = new List<DateReportResponse>();
    }
}