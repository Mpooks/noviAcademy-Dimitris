namespace WorldRank;

public class Player : IPlayer
{
    private readonly Dictionary<Currency, Wallet> _wallets = new();

    public int Id { get; internal set; }
    public string Name { get; }
    public int Score { get; private set; }

    public IReadOnlyDictionary<Currency, Wallet> Wallets => _wallets;

    public Player(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name cannot be empty.", nameof(name));
        }

        Name = name.Trim();
        Score = 0;
    }

    public void AddScore(int points)
    {
        if (points < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(points), "Score points cannot be negative.");
        }

        Score += points;
    }

    internal void AddWallet(Wallet wallet)
    {
        if (wallet == null)
        {
            throw new ArgumentNullException(nameof(wallet));
        }

        _wallets.Add(wallet.Currency, wallet);
    }

    public override string ToString()
    {
        return $"Id: {Id} | Name: {Name} | Score: {Score}";
    }
}