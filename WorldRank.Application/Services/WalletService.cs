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

        public void AddWalletToPlayer(int playerId, Currency currency, decimal startBalance)
        {
           if (_playerRepository.FindPlayer(playerId) is null)
                throw new PlayerNotFoundException(playerId);

                var wallet = new Wallet(playerId, currency, startBalance);
                _walletRepository.Add(wallet);
        }

        public List<Wallet> GetWalletsOfPlayer(int playerId)
        {
            return _walletRepository
                .GetAllWalletsByPlayerId(playerId);
        }

        public void DepositToWallet(int playerId, Currency currency, decimal amount)
        {
            _walletRepository.Deposit(playerId, currency, amount);   
        }

        public void WithdrawFromWallet(int playerId, Currency currency, decimal amount)
        {
           _walletRepository.Withdraw(playerId, currency, amount);
        }

        public void BlockWallet(int playerId, Currency currency)
        {
            _walletRepository.Block(playerId, currency);
        }

        public void UnblockWallet(int playerId, Currency currency)
        {
            _walletRepository.Unblock(playerId, currency);
        }

        public void UpdateWalletBalance(int playerId, Currency currency, decimal newBalance)
        {
            _walletRepository.UpdateBalance(playerId, currency, newBalance);

        }

        public void ApplyFundsOperation(int playerId, Currency currency, decimal amount, FundsOperation operation)
        {
            var strategy = _fundsStrategies
                .FirstOrDefault(strategy => strategy.Operation == operation);
            if (strategy is null)
            {
                throw new InvalidOperationException(
                    $"No strategy registered for {operation}.");
            }
            var wallet = _walletRepository.GetWallet(playerId, currency);
            strategy.Execute(wallet, amount);
            _logger.LogInformation("Applied {Strategy} to player {PlayerId} {Currency} wallet", strategy.GetType().Name,
                playerId, currency);
        }
    }
}
