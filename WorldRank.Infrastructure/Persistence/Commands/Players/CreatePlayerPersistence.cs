using WorldRank.Application.Infrastructure;
using WorldRank.Domain.Entities.Player;
using WorldRank.Infrastructure.Data;

namespace WorldRank.Infrastructure.Persistence.Commands.Players
{
    public class CreatePlayerPersistence : ICreatePlayerPersistence
    {
        private readonly WorldRankDbContext _dbContext;
        public CreatePlayerPersistence(WorldRankDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Persist(Player player, CancellationToken cancellationToken)
        {
            await _dbContext.Players.AddAsync(player, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
