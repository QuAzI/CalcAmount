using CalcAmount.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CalcAmount.Services
{
    public interface ICurrenciesService
    {
        Task<IReadOnlyDictionary<string, string>> GetCurrenciesAsync();
        Task<CurrenciesResponse> GetRatesFromDate(IReadOnlyList<string> currencies, DateTime startingDate);
    }
}