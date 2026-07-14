using WorldRank.Application.Strategies;
using WorldRank.Domain.Entities.Wallets;

namespace WorldRank.Application.Interfaces
{
    public interface IFundsStrategy
    {
        FundsOperation Operation {  get; }
        void Execute(Wallet wallet, decimal amount);
    }
}
