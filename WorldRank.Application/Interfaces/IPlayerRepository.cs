using WorldRank.Domain.Entities.Player;

namespace WorldRank.Application.Interfaces
{
	public interface IPlayerRepository
	{
		Task AddPlayerAsync(Player player, CancellationToken cancellationToken);

		Task<List<Player>> GetAllPlayersAsync(CancellationToken cancellationToken);

		Task DeletePlayerAsync(int playerId, CancellationToken cancellationToken);

		Task<Player?> FindPlayerAsync(int playerId, CancellationToken cancellationToken);

		Task<List<IGrouping<int, Player>>> GroupPlayersByScoreAsync(CancellationToken cancellationToken);
	}
}