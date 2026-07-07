namespace WorldRank;

public class Player : IPlayer
{
    public int Id { get; private set; }
    public string Name { get; set; }
    public int Score { get; set; }
    public Dictionary<Currency, Wallet> Wallets { get; set; } = new Dictionary<Currency, Wallet>();

    public Player(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public void AddScore (int points)
    {
        if (points < 0)
            throw new ArgumentOutOfRangeException(nameof(points));
        Score = points;
    }
    public override string ToString() => $"Id : {Id} | {Name} (Score: ({Score}))";
}