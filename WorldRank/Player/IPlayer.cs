using WorldRank;

namespace WorldRank
{
    public interface IPlayer
    {
        string Name { get; set; }
        int Score { get; set; }
        Dictionary <Currency, Wallet> Wallets  { get; set; }
    }
}
