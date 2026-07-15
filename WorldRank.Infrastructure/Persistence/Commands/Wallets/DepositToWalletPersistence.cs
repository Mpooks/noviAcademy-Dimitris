
using Microsoft.EntityFrameworkCore;
using WorldRank.Application.Infrastructure;
using WorldRank.Domain.Entities.Wallets;
using WorldRank.Infrastructure.Data;

namespace WorldRank.Infrastructure.Persistence.Commands.Wallets
{
    public class DepositToWalletPersistence : IDepositToWalletPersistence
    {
        private readonly WorldRankDbContext _dbContext;

        public DepositToWalletPersistence(WorldRankDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Wallet?> DepositAsync(int walletId, decimal amount, CancellationToken cancellationToken)
        {
            var wallets = await _dbContext.Wallets.FirstOrDefaultAsync(w => w.Id == walletId, cancellationToken);

            if (wallets is null)
            {
                return null;
            }

            wallets.Deposit(amount);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return wallets;
        }
    }
}
