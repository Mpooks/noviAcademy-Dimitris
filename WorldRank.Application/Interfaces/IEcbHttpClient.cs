namespace WorldRank.Application.Interfaces
{
    public interface IEcbHttpClient
    {
        public Task<IReadOnlyList<CurrencyRateDto>> GetLatestRatesAsync(CancellationToken cancellationToken = default);
    }
}
