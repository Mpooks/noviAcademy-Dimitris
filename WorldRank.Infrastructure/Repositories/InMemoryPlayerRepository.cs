using NLog;
using WorldRank.Application.Interfaces;
using WorldRank.Domain.Entities.Player;

namespace WorldRank.Infrastructure.Repositories
{
	public class InMemoryPlayerRepository : IPlayerRepository
	{
		private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

		private List<Player> _players;

		public InMemoryPlayerRepository()
		{
			_players = new List<Player>();
		}

		public Task AddPlayerAsync(Player player, CancellationToken cancellationToken)
		{
            cancellationToken.ThrowIfCancellationRequested();

            _players.Add(player);

            return Task.CompletedTask;
        }

		public Task<List<Player>> GetAllPlayersAsync(CancellationToken cancellationToken)
		{
            cancellationToken.ThrowIfCancellationRequested();

            return Task.FromResult(_players.ToList());
        }

		public Task DeletePlayerAsync(int playerId, CancellationToken cancellationToken)
		{
            cancellationToken.ThrowIfCancellationRequested();
            var player = _players.FirstOrDefault(item => item.Id == playerId);

            if (player is not null)
                _players.Remove(player);

            return Task.CompletedTask;
        }

		public Task<Player?> FindPlayerAsync(int playerId, CancellationToken cancellationToken)
		{
            cancellationToken.ThrowIfCancellationRequested();
			Player? player = _players.FirstOrDefault(item => item.Id == playerId);

			return Task.FromResult(player);
        }

		public Task<List<IGrouping<int, Player>>> GroupPlayersByScoreAsync(CancellationToken cancellationToken)
		{
            cancellationToken.ThrowIfCancellationRequested();
            var groups = _players.GroupBy(player => player.Score).OrderByDescending(group => group.Key).ToList();

            return Task.FromResult(groups);
        }
	}
}
