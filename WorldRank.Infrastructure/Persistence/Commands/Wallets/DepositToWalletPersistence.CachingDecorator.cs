using System;
using System.Collections.Generic;
using System.Text;
using WorldRank.Application.Infrastructure;
using WorldRank.Application.Interfaces;
using WorldRank.Domain.Entities.Wallets;

namespace WorldRank.Infrastructure.Persistence.Commands.Wallets
{
    public class DepositToWalletPersistenceCachingDecorator : IDepositToWalletPersistence
    {
        private static readonly TimeSpan CacheDuration = TimeSpan.FromSeconds(60);
        private readonly IDepositToWalletPersistence _inner;
        private readonly ICache _cache;

        public DepositToWalletPersistenceCachingDecorator(IDepositToWalletPersistence inner, ICache cache)
        {
            _inner = inner;
            _cache = cache;
        }

        private static string WalletKey(int walletId)
        {
            return $"wallet:{walletId}";
        }
        private static string AllWalletsKey(int playerId)
        {
            return $"wallets:player:{playerId}"; 
        }
        public async Task<Wallet?> DepositAsync(int walletId, decimal amount, CancellationToken cancellationToken)
        {
            var wallet = await _inner.DepositAsync(walletId, amount, cancellationToken);
            if (wallet is null) return null;
            _cache.Set(WalletKey(walletId), wallet, CacheDuration);
            _cache.Remove(AllWalletsKey(wallet.PlayerId));

            return wallet;
        }
    }
}
