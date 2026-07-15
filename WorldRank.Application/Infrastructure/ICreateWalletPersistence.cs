using WorldRank.Domain.Entities.Wallets;

namespace WorldRank.Application.Infrastructure
{
    public interface ICreateWalletPersistence
    {
        public Task Persist(Wallet wallet, CancellationToken cancellationToken);
    }
}
