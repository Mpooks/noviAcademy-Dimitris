namespace WorldRank
{
    public interface IPlayerRepository
    {
        Player? FindPlayer(int playerId);
        void AddPlayer(Player player);
        void DeletePlayer(int playerId);
        IEnumerable<IGrouping<int,Player>> GroupPlayersByScore();
        
    }
}
