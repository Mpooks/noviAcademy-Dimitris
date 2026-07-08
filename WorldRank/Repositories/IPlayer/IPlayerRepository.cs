namespace WorldRank;

public interface IPlayerRepository
{
    void AddPlayer(Player player);
    void DeletePlayer(int playerId);
    Player? FindPlayer(int playerId);
    Player? FindByName(string name);
    List<Player> GetAllPlayers();
    IEnumerable<IGrouping<int, Player>> GroupPlayersByScore();
}