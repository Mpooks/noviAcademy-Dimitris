using CurrencyRateE = WorldRank.Domain.Entities.CurrencyRates.CurrencyRates;

namespace WorldRank.Application.Infrastructure
{
    public interface IStoreCurrencyRatesPersistence
    {
        public Task<int> Persist(IReadOnlyCollection<CurrencyRateE> rates, CancellationToken cancellationToken);
    }
}
