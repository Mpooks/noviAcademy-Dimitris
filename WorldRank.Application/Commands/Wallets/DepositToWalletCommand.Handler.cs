using MediatR;
using WorldRank.Application.Infrastructure;
using WorldRank.Domain.Entities.Wallets;

namespace WorldRank.Application.Commands.Wallets
{
    public class DepositToWalletCommandHandler : IRequestHandler<DepositToWalletCommand, Wallet?>
    {
        private readonly IDepositToWalletPersistence _persistence;

        public DepositToWalletCommandHandler(IDepositToWalletPersistence persistence)
        {
            _persistence = persistence;
        }

        public Task<Wallet?> Handle(DepositToWalletCommand req, CancellationToken cancellationToken)
        {
            return _persistence.DepositAsync(req.Id, req.Amount, cancellationToken);
        }
    }
}
