using MediatR;
using WorldRank.Application.Infrastructure;
using WorldRank.Domain.Entities.Player;
using WorldRank.Domain.Entities.Wallets;

namespace WorldRank.Application.Queries.Wallets
{
    public class GetWalletByIdQueryHandler : IRequestHandler<GetWalletByIdQuery, Wallet?>
    {
        private readonly IGetWalletByIdPersistence _persistence;

        public GetWalletByIdQueryHandler(IGetWalletByIdPersistence  persistence)
        {
            _persistence = persistence;
        }
        public async Task<Wallet?> Handle(GetWalletByIdQuery req, CancellationToken cancellationToken)
        {
            return await _persistence.GetByIdAsync(req.WalletId, cancellationToken);
        }
    }
}
