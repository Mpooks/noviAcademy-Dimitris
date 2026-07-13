using Microsoft.EntityFrameworkCore;
using NLog;
using WorldRank.Application.Interfaces;
using WorldRank.Domain.Entities.Enums;
using WorldRank.Domain.Entities.Exceptions;
using WorldRank.Domain.Entities.Wallets;
using WorldRank.Infrastructure.Data;

namespace WorldRank.Infrastructure.Repositories
{
    public class DBWalletRepository : IWalletRepository
    {
        private readonly WorldRankDbContext _dbContext;
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public DBWalletRepository(WorldRankDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(Wallet wallet)
        {
            var exists = _dbContext.Wallets.Any(item => item.PlayerId == wallet.PlayerId && item.Currency == wallet.Currency);

            if (exists)
            {
                throw new DuplicateWalletException(wallet.PlayerId, wallet.Currency);
            }

            _dbContext.Wallets.Add(wallet);
            _dbContext.SaveChanges();
            _logger.Info("Wallet created for player {PlayerId} in {Currency} with balance {Balance}", wallet.PlayerId, wallet.Currency, wallet.Balance);
        }

        public List<Wallet> GetAllWalletsByPlayerId(int playerId)
        {
            return _dbContext.Wallets.AsNoTracking().Where(item => item.PlayerId == playerId).ToList();
        }

        public void UpdateBalance(int playerId, Currency currency, decimal newBalance)
        {
            GetWallet(playerId, currency).SetBalance(newBalance);
            _dbContext.SaveChanges();
            _logger.Info("Player {PlayerId} {Currency} wallet balance set to {Balance}", playerId, currency, newBalance);
        }

        public void Deposit(int playerId, Currency currency, decimal amount)
        {
            var wallet = GetWallet(playerId, currency);
            wallet.Deposit(amount);
            _dbContext.SaveChanges();
            _logger.Info("Deposited {Amount} to player {PlayerId} {Currency} wallet (balance {Balance})", amount, playerId, currency, wallet.Balance);
        }

        public void Withdraw(int playerId, Currency currency, decimal amount)
        {
            var wallet = GetWallet(playerId, currency);
            wallet.Withdraw(amount);
            _dbContext.SaveChanges();
            _logger.Info("Withdrew {Amount} from player {PlayerId} {Currency} wallet (balance {Balance})", amount, playerId, currency, wallet.Balance);
        }

        public void Block(int playerId, Currency currency)
        {
            GetWallet(playerId, currency).Block();
            _dbContext.SaveChanges();
            _logger.Info("Player {PlayerId} {Currency} wallet blocked", playerId, currency);
        }

        public void Unblock(int playerId, Currency currency)
        {
            GetWallet(playerId, currency).Unblock();
            _dbContext.SaveChanges();
            _logger.Info("Player {PlayerId} {Currency} wallet unblocked", playerId, currency);
        }

        public Wallet GetWallet(int playerId, Currency currency)
        {
            var wallet = _dbContext.Wallets.SingleOrDefault(item => item.PlayerId == playerId && item.Currency == currency);

            if (wallet is null)
            {
                throw new WalletNotFoundException(playerId, currency);
            }

            return wallet;
        }

        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }
    }
}
