using Microsoft.Extensions.Logging;
using WorldRank.Application.Infrastructure;
using WorldRank.Application.Interfaces;
using WorldRank.Domain.Entities.Wallets;

namespace WorldRank.Infrastructure.Persistence.Queries
{
    public class GetWalletByIdPersistenceCachingDecorator : IGetWalletByIdPersistence
    {
        private readonly IGetWalletByIdPersistence _inner;
        private readonly ICache _cache;
        private readonly ILogger<GetWalletByIdPersistenceCachingDecorator> _logger;
        private static readonly TimeSpan CacheDuration = TimeSpan.FromSeconds(60);

        public GetWalletByIdPersistenceCachingDecorator(IGetWalletByIdPersistence inner, ICache cache, ILogger<GetWalletByIdPersistenceCachingDecorator> logger)
        {
            _inner = inner;
            _cache = cache;
            _logger = logger;
        }

        private static string WalletKey(int walletId)
        {
            return $"wallet:{walletId}";
        }

        public async Task<Wallet?> GetByIdAsync(int walletId, CancellationToken cancellationToken)
        {
            var key = WalletKey(walletId);
            if(_cache.TryGet(key, out Wallet? cached) && cached is not null)
            {
                _logger.LogInformation("Cache HIT: wallet {walletId}", walletId);
                return cached;
            }
            _logger.LogInformation("Cache MISS: wallet {walletId}", walletId);
            var wallet = await _inner.GetByIdAsync(walletId, cancellationToken);
            if(wallet is not null)
            {
                _cache.Set(key, wallet, CacheDuration);
            }

            return wallet;
        }
    }
}
