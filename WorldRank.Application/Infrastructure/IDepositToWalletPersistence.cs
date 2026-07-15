using WorldRank.Domain.Entities.Wallets;

namespace WorldRank.Application.Infrastructure
{
    public interface IDepositToWalletPersistence
    {
        public Task<Wallet?> DepositAsync(int walletId, decimal amount, CancellationToken cancellationToken);
    }
}
