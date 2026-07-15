using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WorldRank.Application.Infrastructure;
using WorldRank.Application.Interfaces;
using WorldRank.Domain.Entities.Player;

namespace WorldRank.Infrastructure.Persistence.Queries
{
	public class GetAllPersistenceCachingDecorator : IGetAllPersistence
	{
		private const string AllPlayersKey = "players:all";
		private readonly IGetAllPersistence _inner;
		private readonly ICache _cache;
		private readonly ILogger<GetAllPersistenceCachingDecorator> _logger;
		private static readonly TimeSpan CacheDuration = TimeSpan.FromSeconds(60);

		public GetAllPersistenceCachingDecorator(
		IGetAllPersistence inner,
		ICache cache,
		ILogger<GetAllPersistenceCachingDecorator> logger)
		{
			_inner = inner;
			_cache = cache;
			_logger = logger;
		}

		public async Task<List<Player>> GetAllAsync(
		CancellationToken cancellationToken)
		{
			if (_cache.TryGet(
					AllPlayersKey,
					out List<Player>? cached)
				&& cached is not null)
			{
				_logger.LogInformation("Cache HIT: all players");
				return cached;
			}

			_logger.LogInformation("Cache MISS: all players");

			var players = await _inner.GetAllAsync(cancellationToken);

			_cache.Set(
				AllPlayersKey,
				players,
				CacheDuration);

			return players;
		}
	}
}