using WorldRank.Domain.Entities.Enums;
using WorldRank.Domain.Entities.Wallets;

namespace WorldRank.Application.Interfaces
{
	public interface IWalletRepository
	{
		Task AddAsync(Wallet wallet, CancellationToken cancellationToken);

		Task<List<Wallet>> GetAllWalletsByPlayerIdAsync(int playerId, CancellationToken cancellationToken);
        Task<Wallet?> GetWalletByIdAsync(int walletId, CancellationToken cancellationToken);

        Task UpdateBalanceAsync(int playerId, Currency currency, decimal newBalance, CancellationToken cancellationToken);

		Task WithdrawAsync(int playerId, Currency currency, decimal amount, CancellationToken cancellationToken);
        Task BlockAsync(int playerId, Currency currency, CancellationToken cancellationToken);

		Task UnblockAsync(int playerId, Currency currency, CancellationToken cancellationToken);

        Task<Wallet?> GetWalletAsync(int playerId, Currency currency, CancellationToken cancellationToken);

		Task SaveChangesAsync(CancellationToken cancellationToken);
    }
}
