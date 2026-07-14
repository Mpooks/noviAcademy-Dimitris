using WorldRank.Application.Interfaces;
using WorldRank.Domain.Entities.Player;
using Microsoft.Extensions.Logging;

namespace WorldRank.Application.Services
{
    public class PlayerService
    {
        private static readonly TimeSpan CacheDuration =TimeSpan.FromSeconds(60);
        private const string AllPlayersKey = "players:all";
        private readonly ICache _cache;
        private readonly IPlayerRepository _playerRepository;
        private readonly ILogger<PlayerService> _logger;

        public PlayerService(IPlayerRepository playerRepository,ICache cache,ILogger<PlayerService> logger)
        {
            _playerRepository = playerRepository;
            _cache = cache;
            _logger = logger;
        }

        private static string PlayerKey(int playerId)
        {
            return $"player:{playerId}";
        }

        public async Task<int> AddPlayerAsync(string name, int score, CancellationToken cancellationToken)
        {
            var id = await GeneratePlayerIdAsync(cancellationToken);

            var player = new Player(id, name);
            player.AddScore(score);

            await _playerRepository.AddPlayerAsync(player, cancellationToken);

            _cache.Set(PlayerKey(player.Id), player, CacheDuration);
            _cache.Remove(AllPlayersKey);

            return id;
        }

        public async Task<List<Player>> ListPlayersAsync(CancellationToken cancellationToken)
        {
            if (_cache.TryGet(AllPlayersKey,out List<Player>? cached) && cached is not null)
            {
                _logger.LogInformation("Cache HIT: all players");
                return cached;
            }
            _logger.LogInformation("Cache MISS: all players");
            var players = await _playerRepository.GetAllPlayersAsync(cancellationToken);
            _cache.Set(AllPlayersKey, players, CacheDuration);

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

        public async Task<Player?> FindPlayerByIdAsync(int playerId,CancellationToken cancellationToken)
        {
            var key = PlayerKey(playerId);
            if (_cache.TryGet(key,out Player? cached) && cached is not null)
            {
                _logger.LogInformation("Cache HIT: player {PlayerId}",playerId);
                return cached;
            }

            _logger.LogInformation("Cache MISS: player {PlayerId}",playerId);

            var player =await _playerRepository.FindPlayerAsync(playerId,cancellationToken);

            if (player is not null)
            {
                _cache.Set(key,player,CacheDuration);
            }
            return player;
        }

        public async Task DeletePlayerAsync(int playerId,CancellationToken cancellationToken)
        {
            await _playerRepository.DeletePlayerAsync(playerId, cancellationToken);
            _cache.Remove(PlayerKey(playerId));
            _cache.Remove(AllPlayersKey);
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
