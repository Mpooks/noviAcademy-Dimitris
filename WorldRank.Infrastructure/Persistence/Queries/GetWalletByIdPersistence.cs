using Microsoft.EntityFrameworkCore;
using WorldRank.Application.Infrastructure;
using WorldRank.Domain.Entities.Wallets;
using WorldRank.Infrastructure.Data;

namespace WorldRank.Infrastructure.Persistence.Queries
{
    public class GetWalletByIdPersistence : IGetWalletByIdPersistence
    {
        private readonly WorldRankDbContext _dbContext;

        public GetWalletByIdPersistence(WorldRankDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Wallet?> GetByIdAsync(int walletId, CancellationToken cancellationToken)
        {
            return await _dbContext.Wallets.AsNoTracking().FirstOrDefaultAsync(wallet => wallet.Id == walletId, cancellationToken);
        }
    }
}
