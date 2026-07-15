using WorldRank.Gateway.DTOs;

namespace WorldRank.Gateway.Clients;

public interface IEcbHttpClient
{
    public Task<IReadOnlyList<CurrencyRateDto>> GetLatestRatesAsync(CancellationToken cancellationToken = default);
}
