using WorldRank.Application.Interfaces;
using WorldRank.Domain.Entities.Wallets;

namespace WorldRank.Application.Strategies
{
    public class AddFundsStrategy : IFundsStrategy
    {
        public FundsOperation Operation => FundsOperation.Add;

        public void Execute(Wallet wallet, decimal amount) => wallet.Deposit(amount);
    }
}
