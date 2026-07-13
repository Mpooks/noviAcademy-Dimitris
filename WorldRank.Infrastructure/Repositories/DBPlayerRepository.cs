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

        public void AddPlayer(Player player)
        {
            _dbContext.Players.Add(player);
            _dbContext.SaveChanges();
            _logger.Info("Player {PlayerId} ({Name}) added with score {Score}", player.Id, player.Name, player.Score);

        }

        public IEnumerable<Player> GetAllPlayers()
        {
            return _dbContext.Players.AsNoTracking().ToList();
        }

        public void DeletePlayer(int playerId)
        {
            var player = _dbContext.Players.Where(item => item.Id == playerId).FirstOrDefault();

            if (player is null)
            {
                _logger.Warn("Delete skipped: player {PlayerId} not found", playerId);
                return;
            }

            _dbContext.Players.Remove(player);
            _dbContext.SaveChanges();
            _logger.Info("Player {PlayerId} deleted", playerId);

        }

        public Player? FindPlayer(int playerId)
        {
            return _dbContext.Players.AsNoTracking().Where(item => item.Id == playerId).FirstOrDefault();
        }

        public IEnumerable<IGrouping<int, Player>> GroupPlayersByScore()
        {
            return _dbContext.Players
                .AsNoTracking()
                .ToList()
                .GroupBy(player => player.Score)
                .OrderByDescending(group => group.Key);
        }
    }
}
