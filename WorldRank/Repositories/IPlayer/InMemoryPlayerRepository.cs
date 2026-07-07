namespace WorldRank
{
    internal class InMemoryPlayerRepository : IPlayerRepository
    {
        private List<Player> _players;

        public InMemoryPlayerRepository(List<Player> players)
        {
            _players = players;
        }

        public void AddPlayer(Player player)
        {
            _players.Add(player);
        }

        public void DeletePlayer(int playerId)
        {
            var player = _players.Where(x => x.Id == playerId).FirstOrDefault();

            if (player != null) _players.Remove(player);
        }

        public Player? FindPlayer(int playerId)
        {
            var found = _players.Where(x => x.Id == playerId).FirstOrDefault();

            return found;

        }

        public IEnumerable<IGrouping<int, Player>> GroupPlayersByScore()
        {
            return _players.GroupBy(player => player.Score);
        }

    }
}
