using WorldRank.Application.Infrastructure;
using WorldRank.Application.Interfaces;
using WorldRank.Domain.Entities.Player;

namespace WorldRank.Infrastructure.Persistence.Commands.Players
{
    public class CreatePlayerPersistenceCachingDecorator : ICreatePlayerPersistence
    {
        private readonly ICreatePlayerPersistence _inner;
        private readonly ICache _cache;
        private static readonly TimeSpan CacheDuration = TimeSpan.FromSeconds(60);
        private const string AllPlayersKey = "players:all";

        private static string PlayerKey(int playerId)
        {
            return $"player:{playerId}";
        }


        public CreatePlayerPersistenceCachingDecorator(ICreatePlayerPersistence inner, ICache cache)
        {
            _inner = inner;
            _cache = cache;
        }

        public async Task Persist(Player player, CancellationToken cancellationToken)
        {
            await _inner.Persist(player, cancellationToken);

            _cache.Set(PlayerKey(player.Id), player, CacheDuration);
            _cache.Remove(AllPlayersKey);
        }
    }
}
