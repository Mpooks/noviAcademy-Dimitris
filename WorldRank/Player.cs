namespace WorldRank;

public class Player
{
    public Guid Id { get; }
    public string Name { get; set; }
    public int Score { get; private set; }

    public Player(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty.", nameof(name));

        Id = Guid.NewGuid();
        Name = name;
        Score = 0;
    }

    public void AddScore (int points)
    {
        if (points < 0)
            throw new ArgumentOutOfRangeException(nameof(points));
        Score += points;
    }
    public override string ToString() => $"{Name} (Score: ({Score}))";
}