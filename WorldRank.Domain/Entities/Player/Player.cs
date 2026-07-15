namespace WorldRank.Domain.Entities.Player;

public class Player : IPlayer
{
	public int Id { get; }
	public string Name { get; private set; }
	public int Score { get; private set; }

	private Player()
	{
		Name = string.Empty;
    }

	private Player(int id, string name)
	{
		if (string.IsNullOrWhiteSpace(name))
			throw new ArgumentException("Name cannot be empty.", nameof(name));

		Id = id;
		Name = name;
		Score = 0;
	}

    public static Player CreateNew(int id,string name,int score)
    {
        var player = new Player(id, name);
        player.AddScore(score);

        return player;
    }



    public void AddScore(int points)
	{
		if (points < 0)
			throw new ArgumentOutOfRangeException(nameof(points), "Points cannot be negative.");

		Score += points;
	}

	public override string ToString() => $"[{Id}] {Name} - Score: {Score}";
}
