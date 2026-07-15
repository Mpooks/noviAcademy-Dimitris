using MediatR;
using WorldRank.Domain.Entities.Wallets;

namespace WorldRank.Application.Queries.Wallets
{
    public record GetWalletByIdQuery(int WalletId) : IRequest<Wallet?>;
}
