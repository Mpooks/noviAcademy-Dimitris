using System.Runtime.ConstrainedExecution;
using WorldRank;

var players = new List<Player>();

bool running = true;

var defId = 1;

IWalletRepository walletRepo = new InMemoryWalletRepository(players);
IPlayerRepository playerRepo = new InMemoryPlayerRepository(players);

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
            AddWallet();
            break;
        case "5":
            GetWallets();
            break;
        case "6":
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
        Console.WriteLine("4. Add a wallet to a player");
        Console.WriteLine("5. View a player's wallets");
        Console.WriteLine("6. Exit");
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

        Console.WriteLine("Add the player's score");
        var playerScore = Console.ReadLine();
        
        if (!int.TryParse(playerScore, out var initScore))
        {
            Console.WriteLine("Score must be a whole number.");
            return;
        }

        var player = new Player(defId++, name.Trim());

        player.AddScore(initScore);
        playerRepo.AddPlayer(player);

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
    
void AddWallet()
{
    Console.WriteLine("Give a player id: ");
    var pId = Console.ReadLine();
    Console.WriteLine("Enter one of the following currencies: 0 for 'EUR', 1 for 'USD', 2 for 'JPY'");
    var curr = Console.ReadLine();
    Currency curDef = Currency.EUR;

    switch (curr)
    {
        case "0":
        default:
            curDef = Currency.EUR;
            break;
        case "1":
            curDef = Currency.USD;
            break;
        case "2":
            curDef = Currency.JPY;
            break;
    }

    int.TryParse(pId, out var playerId);
    {
        walletRepo.Add(new Wallet(100, curDef, false), playerId);
    }
}


void GetWallets()
{
    Console.Write("Give a player id: ");
    var input = Console.ReadLine();
    if (!int.TryParse(input, out var playerId))
    {
        Console.WriteLine("Id is not a number.");
        return;
    }

    var wallets = walletRepo.GetByPlayer(playerId);
    if (wallets.Count == 0)
    {
        Console.WriteLine("No wallets found for this player.");
        return;
    }

    for (int i = 0; i < wallets.Count; i++)
    {
        Console.WriteLine($"Wallet Number {i + 1}: {wallets[i]}");
    }
}
