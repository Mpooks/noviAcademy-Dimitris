using NLog;
using WorldRank.Application.Interfaces;
using WorldRank.Domain.Entities.Player;
using WorldRank.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace WorldRank.Infrastructure.Repositories
{
    public class DBPlayerRepository : IPlayerRepository
    {
        private readonly WorldRankDbContext _dbContext;
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public DBPlayerRepository(WorldRankDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddPlayerAsync(Player player, CancellationToken cancellationToken)
        {
            await _dbContext.Players.AddAsync(player, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            _logger.Info("Player {PlayerId} ({Name}) added with score {Score}", player.Id, player.Name, player.Score);

        }

        public async Task<List<Player>> GetAllPlayersAsync(CancellationToken cancellationToken)
        {
            return await _dbContext.Players.AsNoTracking().ToListAsync(cancellationToken);
        }

        public async Task DeletePlayerAsync(int playerId, CancellationToken cancellationToken)
        {
            var player = await _dbContext.Players.Where(item => item.Id == playerId).FirstOrDefaultAsync(cancellationToken);

            if (player is null)
            {
                _logger.Warn("Delete skipped: player {PlayerId} not found", playerId);
                return;
            }

            _dbContext.Players.Remove(player);
            await _dbContext.SaveChangesAsync(cancellationToken);
            _logger.Info("Player {PlayerId} deleted", playerId);

        }

        public async Task<Player?> FindPlayerAsync(int playerId, CancellationToken cancellationToken)
        {
            return await _dbContext.Players.AsNoTracking().Where(item => item.Id == playerId).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<List<IGrouping<int, Player>>> GroupPlayersByScoreAsync(CancellationToken cancellationToken)
        {
            var players = await _dbContext.Players.AsNoTracking().ToListAsync(cancellationToken);

            return players.GroupBy(player => player.Score).OrderByDescending(group => group.Key).ToList();
        }
    }
}
