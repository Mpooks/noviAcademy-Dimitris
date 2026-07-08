namespace WorldRank;

internal class InMemoryPlayerRepository : IPlayerRepository
{
    private readonly List<Player> _players;
    private int _nextId = 1;

    public InMemoryPlayerRepository(List<Player> players)
    {
        _players = players;
    }

    public void AddPlayer(Player player)
    {
        player.Id = _nextId++;
        _players.Add(player);
    }

    public void DeletePlayer(int playerId)
    {
        var player = _players.FirstOrDefault(x => x.Id == playerId);

        if (player != null)
        {
            _players.Remove(player);
        }
    }

    public Player? FindPlayer(int playerId)
    {
        return _players.FirstOrDefault(x => x.Id == playerId);
    }

    public Player? FindByName(string name)
    {
        return _players.FirstOrDefault(player =>
            player.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }

    public List<Player> GetAllPlayers()
    {
        return _players;
    }

    public IEnumerable<IGrouping<int, Player>> GroupPlayersByScore()
    {
        return _players.GroupBy(player => player.Score);
    }
}