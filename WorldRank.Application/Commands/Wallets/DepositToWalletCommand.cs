using MediatR;
using WorldRank.Domain.Entities.Wallets;

namespace WorldRank.Application.Commands.Wallets
{
    public record DepositToWalletCommand(int Id, decimal Amount) : IRequest<Wallet?>;
}
