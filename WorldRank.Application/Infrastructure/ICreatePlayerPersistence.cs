using WorldRank.Domain.Entities.Player;

namespace WorldRank.Application.Infrastructure
{
    public interface ICreatePlayerPersistence
    {
        public Task Persist(Player player, CancellationToken cancellationToken);
    }
}
