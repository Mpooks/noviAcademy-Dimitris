namespace WorldRank.Domain.Entities.CurrencyRates;

public  class CurrencyRates
{
    public string Currency { get; private set; }
    public decimal Rate { get; private set; }
    public DateTime Date { get; private set; }

    private CurrencyRates()
    {
        Currency = string.Empty;
    }

    public CurrencyRates(string currency, decimal rate, DateTime date)
    {
        Currency = currency;
        Rate = rate;
        Date = date;
    }
}
