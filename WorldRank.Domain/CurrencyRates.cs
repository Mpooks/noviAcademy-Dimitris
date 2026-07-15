using System;
using System.Collections.Generic;
using System.Text;

namespace WorldRank.Domain
{
    public  class CurrencyRates
    {
        public CurrencyRates(string currency, decimal rate, DateTime date)
        {
            Currency = currency;
            Rate = rate;
            this.date = date;
        }

        public string Currency { get; }
        public decimal Rate { get; }
        public DateTime date { get; }

        
    }
}
