using Microsoft.EntityFrameworkCore;
using WorldRank.Domain.Entities.Exceptions;
using WorldRank.Domain.Entities.Wallets;
using WorldRank.Infrastructure.Data;
using WorldRank.Application.Infrastructure;

namespace WorldRank.Infrastructure.Persistence.Commands.Wallets
{
    public class CreateWalletPersistence : ICreateWalletPersistence
    {
        private readonly WorldRankDbContext _dbContext;

        public CreateWalletPersistence(WorldRankDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Persist(Wallet wallet, CancellationToken cancellationToken)
        {
            var exists = await _dbContext.Wallets.AnyAsync(item => item.PlayerId == wallet.PlayerId && item.Currency == wallet.Currency, cancellationToken);
            if (exists)
            {
                throw new DuplicateWalletException(wallet.PlayerId, wallet.Currency);
            }
            await _dbContext.Wallets.AddAsync(wallet, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
