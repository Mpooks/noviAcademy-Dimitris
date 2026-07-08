namespace WorldRank;

public interface IPlayer
{
    int Id { get; }
    string Name { get; }
    int Score { get; }
    IReadOnlyDictionary<Currency, Wallet> Wallets { get; }
}