
using WorldRank.Domain.Entities.Wallets;

namespace WorldRank.Application.Infrastructure
{
    public interface IGetWalletByIdPersistence
    {
        Task <Wallet?> GetByIdAsync(int walletId, CancellationToken cancellationToken);
    }
}
