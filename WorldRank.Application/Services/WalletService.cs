using Microsoft.Extensions.Logging;
using WorldRank.Application.Interfaces;
using WorldRank.Application.Strategies;
using WorldRank.Domain.Entities.Enums;
using WorldRank.Domain.Entities.Exceptions;
using WorldRank.Domain.Entities.Wallets;

namespace WorldRank.Application.Services
{
    public class WalletService
    {
        private readonly IWalletRepository _walletRepository;
        private readonly IPlayerRepository _playerRepository;
        private readonly IEnumerable<IFundsStrategy> _fundsStrategies;
        private readonly ILogger<WalletService> _logger;
        private static readonly TimeSpan CacheDuration = TimeSpan.FromSeconds(60);
        private readonly ICache _cache;
        private static string WalletKey(int walletId)
        {
            return $"wallet:{walletId}";
        }

        private static string PlayerWalletsKey(int playerId)
        {
            return $"wallets:player:{playerId}";
        }

        public WalletService(
            IWalletRepository walletRepository,
            IPlayerRepository playerRepository,
            IEnumerable<IFundsStrategy> fundsStrategies,
            ILogger<WalletService> logger,
            ICache cache)
        {
            _walletRepository = walletRepository;
            _playerRepository = playerRepository;
            _fundsStrategies = fundsStrategies;
            _logger = logger;
            _cache = cache;
        }

        public async Task<Wallet> AddWalletToPlayerAsync(int playerId,Currency currency,decimal startBalance,CancellationToken cancellationToken)
        {
            var player = await _playerRepository.FindPlayerAsync(playerId,cancellationToken);
            if (player is null)
            {
                throw new PlayerNotFoundException(playerId);
            }

            var wallet = new Wallet(playerId,currency,startBalance);
            await _walletRepository.AddAsync(wallet,cancellationToken);
            _cache.Set(WalletKey(wallet.Id),wallet,CacheDuration);
            _cache.Remove(PlayerWalletsKey(wallet.PlayerId));
            return wallet;
        }

        public async Task<List<Wallet>> GetWalletsOfPlayerAsync(int playerId,CancellationToken cancellationToken)
        {
            var key = PlayerWalletsKey(playerId);
            if (_cache.TryGet(key,out List<Wallet>? cached) && cached is not null)
            {
                _logger.LogInformation("Cache HIT: wallets for player {PlayerId}",playerId);

                return cached;
            }
            _logger.LogInformation("Cache MISS: wallets for player {PlayerId}",playerId);
            var wallets = await _walletRepository.GetAllWalletsByPlayerIdAsync(playerId, cancellationToken);
            _cache.Set(key,wallets,CacheDuration);

            return wallets;
        }

        public async Task<Wallet?> GetWalletByIdAsync(int walletId,CancellationToken cancellationToken)
        {
            var key = WalletKey(walletId);

            if (_cache.TryGet(key,out Wallet? cached) && cached is not null)
            {
                _logger.LogInformation("Cache HIT: wallet {WalletId}",walletId);
                return cached;
            }

            _logger.LogInformation("Cache MISS: wallet {WalletId}",walletId);

            var wallet = await _walletRepository.GetWalletByIdAsync(walletId,cancellationToken);

            if (wallet is not null)
            {
                _cache.Set(key,wallet,CacheDuration);
            }
            return wallet;
        }

        public async Task<Wallet?> DepositToWalletAsync(int walletId,decimal amount,CancellationToken cancellationToken)
        {
            var wallet = await _walletRepository.GetWalletByIdAsync(walletId,cancellationToken);

            if (wallet is null) { return null; }
            wallet.Deposit(amount); 
            await _walletRepository.SaveChangesAsync(cancellationToken);
            _cache.Set(WalletKey(walletId), wallet, CacheDuration);
            _cache.Remove(PlayerWalletsKey(wallet.PlayerId));
            return wallet;
        }

        public async Task WithdrawFromWalletAsync(int playerId,Currency currency,decimal amount,CancellationToken cancellationToken)
        {
            var wallet = await _walletRepository.GetWalletAsync(playerId,currency,cancellationToken);
            if (wallet is null)
            {
                throw new WalletNotFoundException(playerId,currency);
            }
            wallet.Withdraw(amount);
            await _walletRepository.SaveChangesAsync(cancellationToken);

            _cache.Set(WalletKey(wallet.Id),wallet,CacheDuration);

            _cache.Remove(PlayerWalletsKey(wallet.PlayerId));
        }

        public async Task BlockWalletAsync(int playerId, Currency currency, CancellationToken cancellationToken)
        {
            var wallet = await _walletRepository.GetWalletAsync(playerId, currency, cancellationToken);
            if (wallet is null)
            {
                throw new WalletNotFoundException(playerId, currency);
            }

            wallet.Block();
            await _walletRepository.SaveChangesAsync(cancellationToken);

            _cache.Set(WalletKey(wallet.Id), wallet, CacheDuration);

            _cache.Remove(PlayerWalletsKey(wallet.PlayerId));
        }

        public async Task UnblockWalletAsync(int playerId, Currency currency, CancellationToken cancellationToken)
        {
            var wallet = await _walletRepository.GetWalletAsync(playerId, currency, cancellationToken);
            if (wallet is null)
            {
                throw new WalletNotFoundException(playerId, currency);
            }

            wallet.Unblock();
            await _walletRepository.SaveChangesAsync(cancellationToken);

            _cache.Set(WalletKey(wallet.Id), wallet, CacheDuration);

            _cache.Remove(PlayerWalletsKey(wallet.PlayerId));
        }

        public async Task UpdateWalletBalanceAsync(int playerId, Currency currency, decimal newBalance, CancellationToken cancellationToken)
        {
            var wallet = await _walletRepository.GetWalletAsync(playerId, currency, cancellationToken);
            if (wallet is null)
            {
                throw new WalletNotFoundException(playerId, currency);
            }

            wallet.SetBalance(newBalance);
            await _walletRepository.SaveChangesAsync(cancellationToken);

            _cache.Set(WalletKey(wallet.Id), wallet, CacheDuration);

            _cache.Remove(PlayerWalletsKey(wallet.PlayerId));
        }

        public async Task ApplyFundsOperationAsync(int playerId, Currency currency, decimal amount, FundsOperation operation, CancellationToken cancellationToken)
        {
            var strategy = _fundsStrategies.FirstOrDefault(strategy => strategy.Operation == operation);
            if (strategy is null)
            {
                throw new InvalidOperationException(
                    $"No strategy registered for {operation}.");
            }

            var wallet = await _walletRepository.GetWalletAsync(playerId, currency, cancellationToken);
            if (wallet is null)
            {
                throw new WalletNotFoundException(playerId,currency);
            }
            strategy.Execute(wallet, amount);
            await _walletRepository.SaveChangesAsync(cancellationToken);
            _cache.Set(WalletKey(wallet.Id),wallet,CacheDuration);
            _cache.Remove(PlayerWalletsKey(wallet.PlayerId));
            _logger.LogInformation("Applied {Strategy} to player {PlayerId} {Currency} wallet",strategy.GetType().Name,playerId,currency);
        }
    }
}
