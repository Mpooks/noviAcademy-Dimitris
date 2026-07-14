using MediatR;
using WorldRank.Application.Infrastructure;
using WorldRank.Application.Interfaces;
using WorldRank.Domain.Entities.Exceptions;
using WorldRank.Domain.Entities.Wallets;

namespace WorldRank.Application.Commands.Wallets
{
    public class CreateWalletCommandHandler : IRequestHandler<CreateWalletCommand, int>
    {
        private readonly ICreateWalletPersistence _persistence;
        private readonly IPlayerRepository _playerRepository;

        public CreateWalletCommandHandler(ICreateWalletPersistence persistence, IPlayerRepository playerRepository)
        {
            _persistence = persistence;
            _playerRepository = playerRepository;
        }

        public async Task<int> Handle(CreateWalletCommand req, CancellationToken cancellationToken)
        {
            var player = await _playerRepository.FindPlayerAsync(req.PlayerId, cancellationToken);
            if(player is null)
            {
                throw new PlayerNotFoundException(req.PlayerId);
            }

            var wallet = new Wallet(req.PlayerId, req.Currency, 0m);
            await _persistence.Persist(wallet, cancellationToken);
            return wallet.Id;
        }
    }
}
