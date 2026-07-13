using WorldRank.Application.Interfaces;
using WorldRank.Domain.Entities.Player;
using Microsoft.Extensions.Caching.Memory;

namespace WorldRank.Application.Services
{
    public class PlayerService
    {
        private readonly IMemoryCache _cache;
        private readonly IPlayerRepository _playerRepository;

        public PlayerService(IPlayerRepository playerRepository, IMemoryCache cache)
        {
            _playerRepository = playerRepository;
            _cache = cache;
        }

        public async Task<int> AddPlayerAsync(string name, int score, CancellationToken cancellationToken)
        {
            var id = await GeneratePlayerIdAsync(cancellationToken);

            var player = new Player(id, name);
            player.AddScore(score);

            await _playerRepository.AddPlayerAsync(player, cancellationToken);

            _cache.Remove("AllPlayersKey");

            return id;
        }

        public async Task<List<Player>> ListPlayersAsync(CancellationToken cancellationToken)
        {
            if (_cache.TryGetValue("AllPlayersKey", out List<Player>? cached) && cached is not null)
            {
                return cached;
            }
            var players = await _playerRepository.GetAllPlayersAsync(cancellationToken);
            _cache.Set("AllPlayersKey", players, TimeSpan.FromSeconds(60));

            return players;
        }

        public Task<List<IGrouping<int, Player>>> ListPlayersByScoreAsync(CancellationToken cancellationToken)
        {
            return _playerRepository.GroupPlayersByScoreAsync(cancellationToken);
        }

        public async Task<Player?> FindPlayerByNameAsync(string name, CancellationToken cancellationToken)
        {

            var players = await ListPlayersAsync(cancellationToken);

            return players.FirstOrDefault(player => player.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public Task<Player?> FindPlayerByIdAsync(int playerId, CancellationToken cancellationToken)
        {
            return _playerRepository.FindPlayerAsync(playerId, cancellationToken);
        }

        public async Task DeletePlayerAsync(
            int playerId,
            CancellationToken cancellationToken)
        {
            await _playerRepository.DeletePlayerAsync(
                playerId,
                cancellationToken);

            _cache.Remove("AllPlayersKey");
        }

        private async Task<int> GeneratePlayerIdAsync(CancellationToken cancellationToken)
        {
            var players = await _playerRepository.GetAllPlayersAsync(cancellationToken);

            var existingIds = players
                .Select(player => player.Id)
                .ToHashSet();

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
