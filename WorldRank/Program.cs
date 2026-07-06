using WorldRank;

var players = new List<Player>();

bool running = true;

while (running)
{
    PrintMenu();

    string? option = Console.ReadLine();

    switch (option)
    {
        case "1":
            AddPlayer(players);
            break;
        case "2":
            ShowPlayers(players);
            break;
        case "3":
            FindByName(players);
            break;
        case "4":
            running = false;
            break;
        default:
            Console.WriteLine("That is not a valid option!");
            break;
    }
}


    void PrintMenu()
    {
        Console.WriteLine("==========Player Registry==========");
        Console.WriteLine("1. Add a player");
        Console.WriteLine("2. List all players");
        Console.WriteLine("3. Find a player by name");
        Console.WriteLine("4. Exit");
        Console.WriteLine("===================================");
        Console.WriteLine("Choose one of the options above:");
    }

    void AddPlayer(List<Player> players)
    {
        Console.WriteLine("Add the player's name: ");

        string? name = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("The name you entered was empty!");
            return;
        }

        var player = new Player(name.Trim());

        players.Add(player);

        Console.WriteLine($"Player {name} has been added to the player list.");
    }

    void ShowPlayers(List<Player> players)
    {
        if (players.Count == 0) { Console.WriteLine("There are no players to list!"); }
        else
        {
            Console.WriteLine("==========Players List==========");
            for (int i = 0; i < players.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {players[i]}");
            }
            Console.WriteLine("================================");
        }
    }

    void FindByName(List<Player> players)
    {
        if (players.Count == 0) 
        { 
            Console.WriteLine("There are no players in the list.");
            return;
        }

        Console.WriteLine("Enter a player's name to search for: ");

        string? input = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(input))
        {
            Console.WriteLine("The name you entered was empty!");
            return;
        }

        Player? existingPlayer = players.FirstOrDefault(player => player.Name.Equals(input.Trim(), StringComparison.OrdinalIgnoreCase));

        if (existingPlayer == null) { Console.WriteLine("Player not found"); }
        else
        {
            Console.WriteLine($"{existingPlayer.Name} | Score: {existingPlayer.Score}");
        }
    }
