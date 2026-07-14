using WorldRank.Domain.Entities.Player;

namespace WorldRank.Application.Infrastructure
{
    public interface IGetPlayerByIdPersistence
    {
        Task<Player?> GetByIdAsync(int playerId, CancellationToken cancellationToken);
    }
}
