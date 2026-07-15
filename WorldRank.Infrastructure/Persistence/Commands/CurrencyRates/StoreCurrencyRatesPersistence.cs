using Microsoft.EntityFrameworkCore;
using WorldRank.Application.Infrastructure;
using WorldRank.Infrastructure.Data;
using CurrencyRateE = WorldRank.Domain.Entities.CurrencyRates.CurrencyRates;

namespace WorldRank.Infrastructure.Persistence.Commands.CurrencyRates
{
    public class StoreCurrencyRatesPersistence : IStoreCurrencyRatesPersistence
    {
        private readonly WorldRankDbContext _dbContext;

        public StoreCurrencyRatesPersistence(WorldRankDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> Persist(IReadOnlyCollection<CurrencyRateE> rates, CancellationToken cancellationToken)
        {
            foreach(var rate in rates)
            {
                var existingRate = await _dbContext.CurrencyRates.AnyAsync(x => x.Currency == rate.Currency && x.Date == rate.Date, cancellationToken);

                if (!existingRate)
                {
                    await _dbContext.CurrencyRates.AddAsync(rate, cancellationToken);
                }
            }
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
