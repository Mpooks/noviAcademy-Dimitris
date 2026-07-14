using WorldRank.Application.Interfaces;
using WorldRank.Domain.Entities.Player;

namespace WorldRank.Application.Services
{
    public class PlayerService
    {
        private readonly IPlayerRepository _playerRepository;

        public PlayerService(IPlayerRepository playerRepository)
        {
            _playerRepository = playerRepository;
        }

        public void AddPlayer(string name, int score)
        {
            var player = new Player(GeneratePlayerId(), name);
            player.AddScore(score);
            _playerRepository.AddPlayer(player);
        }

        public List<Player> ListPlayers()
        {
            return _playerRepository
                .GetAllPlayers()
                .ToList();
        }

        public List<IGrouping<int, Player>> ListPlayersByScore()
        {
            return _playerRepository
                .GroupPlayersByScore()
                .ToList();
        }

        public Player? FindPlayerByName(string name)
        {

           return _playerRepository.GetAllPlayers()
                .FirstOrDefault(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public Player? FindPlayerById(int playerId)
        {
            return _playerRepository.FindPlayer(playerId);
        }

        public void DeletePlayer(int playerId)
        {
            _playerRepository.DeletePlayer(playerId);
        }

        private int GeneratePlayerId()
        {
            var existingIds = _playerRepository
                .GetAllPlayers()
                .Select(player => player.Id)
                .ToHashSet();

            int id;

            do
            {
                id = Random.Shared.Next(1, int.MaxValue);
            }
            while (existingIds.Contains(id));

            return id;
        }
    }
}
