using WorldRank.Application.Infrastructure;
using WorldRank.Domain.Entities.Player;
using Microsoft.Extensions.Logging;
using WorldRank.Application.Interfaces;

namespace WorldRank.Infrastructure.Persistence.Queries;

public class GetPlayerByIdPersistenceCachingDecorator : IGetPlayerByIdPersistence
{
    private readonly IGetPlayerByIdPersistence _inner;
    private readonly ICache _cache;
    private readonly ILogger<GetPlayerByIdPersistenceCachingDecorator> _logger;
    private static readonly TimeSpan CacheDuration = TimeSpan.FromSeconds(60);

    private static string PlayerKey(int playerId)
    {
        return $"player:{playerId}";
    }

    public GetPlayerByIdPersistenceCachingDecorator(IGetPlayerByIdPersistence inner, ICache cache,
        ILogger<GetPlayerByIdPersistenceCachingDecorator> logger)
    {
        _inner = inner;
        _cache = cache;
        _logger = logger;
    }

    public async Task<Player?> GetByIdAsync(int playerId, CancellationToken cancellationToken)
    {
        var key = PlayerKey(playerId);
        if (_cache.TryGet(key, out Player? cached) && cached is not null)
        {
            _logger.LogInformation("Cache HIT: player {PlayerId}", playerId);
            return cached;
        }

        _logger.LogInformation("Cache MISS: player {PlayerId}", playerId);

        var player = await _inner.GetByIdAsync(playerId, cancellationToken);

        if (player is not null)
        {
            _cache.Set(key, player, CacheDuration);
        }
        return player;
    }
    
}
