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

        public WalletService(
            IWalletRepository walletRepository,
            IPlayerRepository playerRepository,
            IEnumerable<IFundsStrategy> fundsStrategies,
            ILogger<WalletService> logger)
        {
            _walletRepository = walletRepository;
            _playerRepository = playerRepository;
            _fundsStrategies = fundsStrategies;
            _logger = logger;
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
            return wallet;
        }

        public Task<List<Wallet>> GetWalletsOfPlayerAsync(int playerId, CancellationToken cancellationToken)
        {
            return _walletRepository.GetAllWalletsByPlayerIdAsync(playerId, cancellationToken);
        }

        public Task<Wallet?> GetWalletByIdAsync(int walletId,CancellationToken cancellationToken)
        {
            return _walletRepository.GetWalletByIdAsync(walletId,cancellationToken);
        }

        public async Task<Wallet?> DepositToWalletAsync(int walletId,decimal amount,CancellationToken cancellationToken)
        {
            var wallet = await _walletRepository.GetWalletByIdAsync(walletId,cancellationToken);

            if (wallet is null) { return null; }
            wallet.Deposit(amount); 
            await _walletRepository.SaveChangesAsync(cancellationToken);

            return wallet;
        }

        public Task WithdrawFromWalletAsync(int playerId, Currency currency, decimal amount, CancellationToken cancellationToken)
        {
            return _walletRepository.WithdrawAsync(playerId, currency, amount, cancellationToken);
        }

        public Task BlockWalletAsync(int playerId, Currency currency, CancellationToken cancellationToken)
        {
            return _walletRepository.BlockAsync(playerId, currency, cancellationToken);
        }

        public Task UnblockWalletAsync(int playerId, Currency currency, CancellationToken cancellationToken)
        {
           return _walletRepository.UnblockAsync(playerId, currency, cancellationToken);
        }

        public Task UpdateWalletBalanceAsync(int playerId, Currency currency, decimal newBalance, CancellationToken cancellationToken)
        {
            return _walletRepository.UpdateBalanceAsync(playerId, currency, newBalance, cancellationToken);
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

            _logger.LogInformation(
                "Applied {Strategy} to player {PlayerId} {Currency} wallet",
                strategy.GetType().Name,
                playerId,
                currency);
        }
    }
}
