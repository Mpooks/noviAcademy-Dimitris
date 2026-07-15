using NLog;
using WorldRank.Application.Interfaces;
using WorldRank.Domain.Entities.Enums;
using WorldRank.Domain.Entities.Exceptions;
using WorldRank.Domain.Entities.Wallets;

namespace WorldRank.Infrastructure.Repositories
{
	public class InMemoryWalletRepository : IWalletRepository
	{
		private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

		private readonly List<Wallet> _wallets = new List<Wallet>();

        public Task AddAsync(Wallet wallet, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var exists = _wallets.Any(item => item.PlayerId == wallet.PlayerId && item.Currency == wallet.Currency);

			if (exists)
			{
				throw new DuplicateWalletException(wallet.PlayerId, wallet.Currency);
			}

			_wallets.Add(wallet);
			_logger.Info("Wallet created for player {PlayerId} in {Currency} with balance {Balance}", wallet.PlayerId, wallet.Currency, wallet.Balance);

			return Task.CompletedTask;
		}

        public Task<List<Wallet>> GetAllWalletsByPlayerIdAsync(int playerId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var wallets = _wallets.Where(item => item.PlayerId == playerId).ToList();
			return Task.FromResult(wallets);	
		}

        public Task<Wallet?> GetWalletByIdAsync(int walletId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var wallet = _wallets.FirstOrDefault(wallet => wallet.Id == walletId);
			return Task.FromResult<Wallet?>(wallet);
        }


        public async Task UpdateBalanceAsync(int playerId,Currency currency,decimal newBalance,CancellationToken cancellationToken)
        {
            var wallet = await GetWalletAsync(playerId,currency,cancellationToken);

            if (wallet is null)
            {
                throw new WalletNotFoundException(playerId,currency);
            }

            wallet.SetBalance(newBalance);

            _logger.Info("Player {PlayerId} {Currency} wallet balance set to {Balance}",playerId,currency,newBalance);
        }

        public async Task WithdrawAsync(int playerId, Currency currency, decimal amount, CancellationToken cancellationToken)
        {
            var wallet = await GetWalletAsync(playerId, currency, cancellationToken);
            if (wallet is null) { throw new WalletNotFoundException(playerId, currency); }
            wallet.Withdraw(amount);
            _logger.Info("Withdrew {Amount} from player {PlayerId} {Currency} wallet (balance {Balance})", amount, playerId, currency, wallet.Balance);
		}


        public async Task BlockAsync(int playerId, Currency currency, CancellationToken cancellationToken)
        {
            var wallet = await GetWalletAsync(playerId, currency, cancellationToken);
            if (wallet is null) { throw new WalletNotFoundException(playerId, currency); }
            wallet.Block();
            _logger.Info("Player {PlayerId} {Currency} wallet blocked", playerId, currency);
		}

        public async Task UnblockAsync(int playerId, Currency currency, CancellationToken cancellationToken)
        {
            var wallet = await GetWalletAsync(playerId, currency, cancellationToken);
            if (wallet is null) { throw new WalletNotFoundException(playerId, currency); }
            wallet.Unblock();
            _logger.Info("Player {PlayerId} {Currency} wallet unblocked", playerId, currency);
		}

        public Task<Wallet?> GetWalletAsync(int playerId, Currency currency, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var wallet = _wallets.SingleOrDefault(item => item.PlayerId == playerId && item.Currency == currency);
            return Task.FromResult<Wallet?>(wallet);
        }

        public Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.CompletedTask;
        }
    }
}

