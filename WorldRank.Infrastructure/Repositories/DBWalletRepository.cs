using Microsoft.EntityFrameworkCore;
using NLog;
using WorldRank.Application.Interfaces;
using WorldRank.Domain.Entities.Enums;
using WorldRank.Domain.Entities.Exceptions;
using WorldRank.Domain.Entities.Wallets;
using WorldRank.Infrastructure.Data;

namespace WorldRank.Infrastructure.Repositories
{
    public class DBWalletRepository : IWalletRepository
    {
        private readonly WorldRankDbContext _dbContext;
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public DBWalletRepository(WorldRankDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(Wallet wallet, CancellationToken cancellationToken)
        {
            var exists = await _dbContext.Wallets.AnyAsync(item => item.PlayerId == wallet.PlayerId && item.Currency == wallet.Currency, cancellationToken);

            if (exists)
            {
                throw new DuplicateWalletException(wallet.PlayerId, wallet.Currency);
            }

            await _dbContext.Wallets.AddAsync(wallet, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            _logger.Info("Wallet created for player {PlayerId} in {Currency} with balance {Balance}", wallet.PlayerId, wallet.Currency, wallet.Balance);
        }

        public async Task<List<Wallet>> GetAllWalletsByPlayerIdAsync(int playerId, CancellationToken cancellationToken)
        {
            return await _dbContext.Wallets.AsNoTracking().Where(wallet => wallet.PlayerId == playerId).ToListAsync(cancellationToken);
        }

        public async Task<Wallet?> GetWalletByIdAsync(int walletId, CancellationToken cancellationToken)
        {
            return await _dbContext.Wallets.FirstOrDefaultAsync(wallet => wallet.Id == walletId, cancellationToken);
        }

        public async Task UpdateBalanceAsync(int playerId, Currency currency, decimal newBalance, CancellationToken cancellationToken)
        {
            var wallet = await GetWalletAsync(playerId, currency, cancellationToken);
            if (wallet is null) { throw new WalletNotFoundException(playerId, currency); }
            wallet.SetBalance(newBalance);
            await _dbContext.SaveChangesAsync(cancellationToken);
            _logger.Info("Player {PlayerId} {Currency} wallet balance set to {Balance}", playerId, currency, newBalance);
        }

        public async Task WithdrawAsync(int playerId, Currency currency, decimal amount, CancellationToken cancellationToken)
        {
            var wallet = await GetWalletAsync(playerId, currency, cancellationToken);
            if (wallet is null) { throw new WalletNotFoundException(playerId, currency); }
            wallet.Withdraw(amount);
            await _dbContext.SaveChangesAsync(cancellationToken);
            _logger.Info("Withdrew {Amount} from player {PlayerId} {Currency} wallet (balance {Balance})", amount, playerId, currency, wallet.Balance);
        }

        public async Task BlockAsync(int playerId, Currency currency, CancellationToken cancellationToken)
        {
            var wallet = await GetWalletAsync(playerId, currency, cancellationToken);
            if (wallet is null) { throw new WalletNotFoundException(playerId, currency); }
            wallet.Block();
            await _dbContext.SaveChangesAsync(cancellationToken);
            _logger.Info("Player {PlayerId} {Currency} wallet blocked", playerId, currency);
        }

        public async Task UnblockAsync(int playerId, Currency currency, CancellationToken cancellationToken)
        {
            var wallet = await GetWalletAsync(playerId, currency, cancellationToken);
            if (wallet is null) { throw new WalletNotFoundException(playerId, currency); }
            wallet.Unblock();
            await _dbContext.SaveChangesAsync(cancellationToken);
            _logger.Info("Player {PlayerId} {Currency} wallet unblocked", playerId, currency);
        }

        public async Task<Wallet?> GetWalletAsync(int playerId, Currency currency, CancellationToken cancellationToken)
        {
            return await _dbContext.Wallets.SingleOrDefaultAsync(item => item.PlayerId == playerId && item.Currency == currency, cancellationToken);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
