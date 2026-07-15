using WorldRank.Application.Infrastructure;
using WorldRank.Application.Interfaces;
using WorldRank.Domain.Entities.Wallets;

namespace WorldRank.Infrastructure.Persistence.Commands.Wallets
{
    public class CreateWalletPersistenceCachingDecorator : ICreateWalletPersistence
    {
        private readonly ICreateWalletPersistence _inner;
        private readonly ICache _cache;
        private static readonly TimeSpan CacheDuration = TimeSpan.FromSeconds(60);

        public CreateWalletPersistenceCachingDecorator(ICreateWalletPersistence inner, ICache cache)
        {
            _inner = inner;
            _cache = cache;
        }

        public async Task Persist(Wallet wallet, CancellationToken cancellationToken)
        {
            await _inner.Persist(wallet, cancellationToken);
            _cache.Set(WalletKey(wallet.Id), wallet, CacheDuration);
            _cache.Remove(AllWalletsKey(wallet.PlayerId));
        }

        private static string WalletKey(int walletId)
        {
            return $"wallet:{walletId}";
        }

        private static string AllWalletsKey(int playerId)
        {
            return $"wallets:player:{playerId}";
        }
    }
}
