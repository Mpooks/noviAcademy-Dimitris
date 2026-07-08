using WorldRank;

var players = new List<Player>();

bool running = true;

IWalletRepository walletRepo = new InMemoryWalletRepository(players);
IPlayerRepository playerRepo = new InMemoryPlayerRepository(players);

while (running)
{
    PrintMenu();

    string? option = Console.ReadLine();

    switch (option)
    {
        case "1":
            AddPlayer();
            break;
        case "2":
            ShowPlayers();
            break;
        case "3":
            FindByName();
            break;
        case "4":
            AddWallet();
            break;
        case "5":
            GetWallets();
            break;
        case "6":
            DeletePlayer();
            break;
        case "7":
            GroupPlayersByScore();
            break;
        case "8":
            DepositWallet();
            break;
        case "9":
            WithdrawWallet();
            break;
        case "10":
            ToggleWalletStatus();
            break;
        case "11":
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
    Console.WriteLine("6. Delete a player");
    Console.WriteLine("7. Group players by score");
    Console.WriteLine("8. Deposit to wallet");
    Console.WriteLine("9. Withdraw from wallet");
    Console.WriteLine("10. Toggle wallet block status");
    Console.WriteLine("11. Exit");
    Console.WriteLine("===================================");
    Console.WriteLine("Choose one of the options above:");
}

void AddPlayer()
{
    Console.WriteLine("Add the player's name: ");

    string? name = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(name))
    {
        Console.WriteLine("The name you entered was empty!");
        return;
    }

    Console.WriteLine("Add the player's score:");
    var playerScore = Console.ReadLine();

    if (!int.TryParse(playerScore, out var initScore))
    {
        Console.WriteLine("Score must be a whole number.");
        return;
    }

    var player = new Player(name.Trim());

    player.AddScore(initScore);
    playerRepo.AddPlayer(player);

    Console.WriteLine($"Player {player.Name} has been added with id {player.Id}.");
}

void ShowPlayers()
{
    var allPlayers = playerRepo.GetAllPlayers();

    if (allPlayers.Count == 0)
    {
        Console.WriteLine("There are no players to list!");
        return;
    }

    Console.WriteLine("==========Players List==========");

    for (int i = 0; i < allPlayers.Count; i++)
    {
        Console.WriteLine($"{i + 1}. {allPlayers[i]}");
    }

    Console.WriteLine("================================");
}

void FindByName()
{
    var allPlayers = playerRepo.GetAllPlayers();

    if (allPlayers.Count == 0)
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

    Player? existingPlayer = playerRepo.FindByName(input.Trim());

    if (existingPlayer == null)
    {
        Console.WriteLine("Player not found.");
        return;
    }

    Console.WriteLine(existingPlayer);
}

void DeletePlayer()
{
    Console.WriteLine("Give player id to delete: ");

    var input = Console.ReadLine();

    if (!int.TryParse(input, out var playerId))
    {
        Console.WriteLine("Id is not a number.");
        return;
    }

    var player = playerRepo.FindPlayer(playerId);

    if (player == null)
    {
        Console.WriteLine("There is no player with that id.");
        return;
    }

    playerRepo.DeletePlayer(playerId);

    Console.WriteLine($"The player {player.Name} was deleted successfully.");
}

void GroupPlayersByScore()
{
    var allPlayers = playerRepo.GetAllPlayers();

    if (allPlayers.Count == 0)
    {
        Console.WriteLine("There are no players to group.");
        return;
    }

    var groupedPlayers = playerRepo.GroupPlayersByScore();

    foreach (var group in groupedPlayers)
    {
        Console.WriteLine($"Score: {group.Key}");

        foreach (var player in group)
        {
            Console.WriteLine($"- Id: {player.Id} | Name: {player.Name} | Score: {player.Score}");
        }
    }
}

void AddWallet()
{
    Console.WriteLine("Give a player id: ");
    var pId = Console.ReadLine();

    if (!int.TryParse(pId, out var playerId))
    {
        Console.WriteLine("Id is not a number.");
        return;
    }

    var player = playerRepo.FindPlayer(playerId);

    if (player == null)
    {
        Console.WriteLine("Player not found.");
        return;
    }

    Console.WriteLine("Enter one of the following currencies: 0 for EUR, 1 for USD, 2 for JPY");
    var curr = Console.ReadLine();

    Currency currency;

    switch (curr)
    {
        case "0":
            currency = Currency.EUR;
            break;
        case "1":
            currency = Currency.USD;
            break;
        case "2":
            currency = Currency.JPY;
            break;
        default:
            Console.WriteLine("Invalid currency.");
            return;
    }

    walletRepo.Add(new Wallet(currency), playerId);

    Console.WriteLine($"{currency} wallet added to player {player.Name}.");
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

void DepositWallet()
{
    Console.WriteLine("Give player id: ");
    var input = Console.ReadLine();

    if (!int.TryParse(input, out var playerId))
    {
        Console.WriteLine("The id is not a number.");
        return;
    }

    var player = playerRepo.FindPlayer(playerId);

    if (player == null)
    {
        Console.WriteLine("Player not found.");
        return;
    }

    var wallets = walletRepo.GetByPlayer(playerId);

    if (wallets.Count == 0)
    {
        Console.WriteLine("The user has no wallets.");
        return;
    }

    Console.WriteLine("The wallets are:");

    for (int i = 0; i < wallets.Count; i++)
    {
        Console.WriteLine($"{i + 1}. {wallets[i]}");
    }

    Console.WriteLine("Choose wallet number.");
    var walletNum = Console.ReadLine();

    if (!int.TryParse(walletNum, out var walletNumber))
    {
        Console.WriteLine("You must enter a number.");
        return;
    }

    if (walletNumber < 1 || walletNumber > wallets.Count)
    {
        Console.WriteLine($"Number must be between 1 and {wallets.Count}.");
        return;
    }

    var selectedWallet = wallets[walletNumber - 1];

    Console.WriteLine("How much would you like to deposit?");
    var deposit = Console.ReadLine();

    if (!decimal.TryParse(deposit, out var depositAmount))
    {
        Console.WriteLine("That is not a valid number.");
        return;
    }

    selectedWallet.Deposit(depositAmount);

    Console.WriteLine($"{depositAmount} {selectedWallet.Currency} has been added to the wallet.");
}

void WithdrawWallet()
{
    Console.WriteLine("Give player id: ");
    var input = Console.ReadLine();

    if (!int.TryParse(input, out var playerId))
    {
        Console.WriteLine("The id is not a number.");
        return;
    }

    var player = playerRepo.FindPlayer(playerId);

    if (player == null)
    {
        Console.WriteLine("Player not found.");
        return;
    }

    var wallets = walletRepo.GetByPlayer(playerId);

    if (wallets.Count == 0)
    {
        Console.WriteLine("The user has no wallets.");
        return;
    }

    Console.WriteLine("The wallets are:");

    for (int i = 0; i < wallets.Count; i++)
    {
        Console.WriteLine($"{i + 1}. {wallets[i]}");
    }

    Console.WriteLine("Choose wallet number.");
    var walletNum = Console.ReadLine();

    if (!int.TryParse(walletNum, out var walletNumber))
    {
        Console.WriteLine("You must enter a number.");
        return;
    }

    if (walletNumber < 1 || walletNumber > wallets.Count)
    {
        Console.WriteLine($"Number must be between 1 and {wallets.Count}.");
        return;
    }

    var selectedWallet = wallets[walletNumber - 1];

    Console.WriteLine("How much would you like to withdraw?");
    var withdraw = Console.ReadLine();

    if (!decimal.TryParse(withdraw, out var withdrawAmount))
    {
        Console.WriteLine("That is not a valid number.");
        return;
    }

    selectedWallet.Withdraw(withdrawAmount);

    Console.WriteLine($"{withdrawAmount} {selectedWallet.Currency} has been withdrawn from the wallet.");
}

void ToggleWalletStatus()
{
    Console.WriteLine("Give player id: ");
    var pId = Console.ReadLine();

    if(!int.TryParse(pId, out var playerId))
    {
        Console.WriteLine("The id is not a number.");
        return;
    }

    var player = playerRepo.FindPlayer(playerId);

    if (player == null)
    {
        Console.WriteLine("Player not found");
    }

    var wallets = walletRepo.GetByPlayer(playerId);

    if (wallets.Count == 0)
    {
        Console.WriteLine($"The player with id: {player?.Id} has no wallets.");
    }

    Console.WriteLine("The wallets are:");

    for (int i = 0; i < wallets.Count; i++)
    {
        Console.WriteLine($"{i + 1}. {wallets[i]}");
    }

    Console.WriteLine("Choose wallet number: ");

    var walletChoice = Console.ReadLine();

    if(!int.TryParse(walletChoice, out var walletInput))
    {
        Console.WriteLine("You must enter a valid number");
    }

    if(walletInput < 1 || walletInput > wallets.Count)
    {
        Console.WriteLine("You must enter a valid number");
        return;
    }

    var selWallet = wallets[walletInput - 1];

    string currStatus = selWallet.IsBlocked ? "blocked" : "unblocked";
    Console.WriteLine($"The wallet is {currStatus}. Do you want to change the status? 1-Yes 2-No");

    var sel = Console.ReadLine();
    
    if (sel != "1")
    {
        Console.WriteLine("The status was not changed");
        return;
    }

    selWallet.ToggleBlock();

    Console.WriteLine("The status was changed.");
}