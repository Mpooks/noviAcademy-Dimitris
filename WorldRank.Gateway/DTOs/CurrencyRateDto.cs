namespace WorldRank.Gateway.DTOs

{
    public class CurrencyRateDto
    {
        public string Currency { get; set; }
        public decimal Rate { get; set; }
        public DateTime Date { get; set; }

        public CurrencyRateDto(string currency, decimal rate, DateTime date)
        {
            Currency = currency;
            Rate = rate;
            Date = date;
        }
    }
}
