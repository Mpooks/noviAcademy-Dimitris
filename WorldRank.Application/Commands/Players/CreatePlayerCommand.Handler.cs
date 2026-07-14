using MediatR;
using WorldRank.Application.Infrastructure;
using WorldRank.Application.Interfaces;
using WorldRank.Application.Services;
using WorldRank.Domain.Entities.Player;

namespace WorldRank.Application.Commands.Players
{
    public class CreatePlayerCommandHandler : IRequestHandler<CreatePlayerCommand, int>
    {
        private readonly IPlayerRepository _playerRepository;
        private readonly ICreatePlayerPersistence _playerPersistence;
        public CreatePlayerCommandHandler(ICreatePlayerPersistence playerPersistence,IPlayerRepository playerRepository)
        {
            _playerPersistence = playerPersistence;
            _playerRepository = playerRepository;
        }

        public async Task<int> Handle(CreatePlayerCommand req,CancellationToken cancellationToken)
        {
            var id = await GeneratePlayerIdAsync(cancellationToken);

            var player = Player.CreateNew(id, req.Name, req.Score);

            await _playerPersistence.Persist(player, cancellationToken);
            
            return player.Id;
        }

        private async Task<int> GeneratePlayerIdAsync(CancellationToken cancellationToken)
        {
            var players = await _playerRepository.GetAllPlayersAsync(cancellationToken);
            var existingIds = players.Select(player => player.Id).ToHashSet();
            int id;
            do
            {
                id = Random.Shared.Next(1, int.MaxValue);
            }
            while (existingIds.Contains(id));

            return id;
        }
    }
}
