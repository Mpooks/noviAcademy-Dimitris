using Microsoft.Extensions.DependencyInjection;
using NLog;
using WorldRank;
using WorldRank.Application.Services;

var logger = LogManager.GetCurrentClassLogger();

var services = new ServiceCollection();
services.AddWorldRank();

using var serviceProvider = services.BuildServiceProvider();
var playerService = serviceProvider.GetRequiredService<PlayerService>();
var walletService = serviceProvider.GetRequiredService<WalletService>();


logger.Info("Application started.");

while (true)
{
	Console.WriteLine("\n=== WorldRank Player Registry ===");
	Console.WriteLine("--- Players ---");
	Console.WriteLine("1. Add player");
	Console.WriteLine("2. List all players");
	Console.WriteLine("3. List players grouped by score");
	Console.WriteLine("4. Find player by name");
	Console.WriteLine("5. Find player by id");
	Console.WriteLine("6. Delete player");
	Console.WriteLine("--- Wallets ---");
	Console.WriteLine("7. Add wallet to player");
	Console.WriteLine("8. Show player wallets");
	Console.WriteLine("9. Deposit to wallet");
	Console.WriteLine("10. Withdraw from wallet");
	Console.WriteLine("11. Block wallet");
	Console.WriteLine("12. Unblock wallet");
	Console.WriteLine("13. Update wallet balance");
	Console.WriteLine("14. Apply funds operation strategy");
	Console.WriteLine("0. Exit");
	Console.Write("> ");

	Action? action = Console.ReadLine() switch
	{
		"1" => AddPlayer,
		"2" => ListPlayers,
		"3" => ListPlayersByScore,
		"4" => FindPlayerByName,
		"5" => FindPlayerById,
		"6" => DeletePlayer,
		"7" => AddWalletToPlayer,
		"8" => GetWalletsOfPlayer,
		"9" => DepositToWallet,
		"10" =>WithdrawFromWallet,
		"11" =>BlockWallet,
		"12" =>UnblockWallet,
		"13" =>UpdateWalletBalance,
		"14" =>ApplyFundsOperation,
		"0" => null,
		_ => () => Console.WriteLine("Unknown option.")
	};

	if (action is null)
	{
		logger.Info("Application exiting.");
		LogManager.Shutdown(); // flush file writes before exit
		return; // "0" selected — exit
	}

	try
	{
		action();
	}
	catch (Exception ex)
	{
		// Safety net: log any exception the specific handlers did not catch, and keep the app running.
		logger.Error(ex, "Unexpected error while handling a menu action");
		Console.WriteLine($"Unexpected error: {ex.Message}");
	}

    void AddPlayer()
    {
        var name = ConsolePrompts.PromptName();
        var score = ConsolePrompts.PromptScore();

        if (name is null || score is null)
            return;

        playerService.AddPlayer(name, score.Value);

        Console.WriteLine("Player added successfully.");
    }

    void ListPlayers()
    {
        var players = playerService.ListPlayers();

        if (players.Count == 0)
        {
            Console.WriteLine(
                "No players registered.");

            return;
        }

        foreach (var player in players)
            Console.WriteLine(player);
    }

    void ListPlayersByScore()
    {
        var groups =
            playerService.ListPlayersByScore();

        if (groups.Count == 0)
        {
            Console.WriteLine(
                "No players registered.");

            return;
        }

        foreach (var group in groups)
        {
            Console.WriteLine(
                $"Score {group.Key}:");

            foreach (var player in group)
                Console.WriteLine($"  {player}");
        }
    }

    void FindPlayerByName()
    {
        var name = ConsolePrompts.PromptName();
        if (name is null)
            return;

        var player =
            playerService.FindPlayerByName(name);

        if (player is null)
        {
            Console.WriteLine("No player found.");
            return;
        }

        Console.WriteLine(player);
    }

    void FindPlayerById()
    {
        var playerId =
            ConsolePrompts.PromptPlayerId();

        if (playerId is null)
            return;

        var player =
            playerService.FindPlayerById(
                playerId.Value);

        if (player is null)
        {
            Console.WriteLine("No player found.");
            return;
        }

        Console.WriteLine(player);
    }

    void DeletePlayer()
    {
        var playerId =
            ConsolePrompts.PromptPlayerId();

        if (playerId is null)
            return;

        playerService.DeletePlayer(
            playerId.Value);

        Console.WriteLine(
            "Player deleted if it existed.");
    }

    void AddWalletToPlayer()
    {
        var playerId =
            ConsolePrompts.PromptPlayerId();

        if (playerId is null)
            return;

        var currency =
            ConsolePrompts.PromptCurrency();

        if (currency is null)
            return;

        var balance =
            ConsolePrompts.PromptAmount(
                "Initial balance");

        if (balance is null)
            return;

        walletService.AddWalletToPlayer(
            playerId.Value,
            currency.Value,
            balance.Value);

        Console.WriteLine(
            "Wallet added successfully.");
    }

    void GetWalletsOfPlayer()
    {
        var playerId = ConsolePrompts.PromptPlayerId();
        if (playerId is null)
            return;

        var wallets = walletService.GetWalletsOfPlayer(playerId.Value);
        if (wallets.Count == 0)
        {
            Console.WriteLine("No wallets found for this player.");
            return;
        }

        for (var index = 0;index < wallets.Count;index++)
        {
            Console.WriteLine($"Wallet {index + 1}: {wallets[index]}");
        }
    }

    void DepositToWallet()
    {
        var playerId = ConsolePrompts.PromptPlayerId();
        if (playerId is null)
            return;

        var currency = ConsolePrompts.PromptCurrency();
        if (currency is null)
            return;

        var amount =ConsolePrompts.PromptAmount("Amount to deposit");
        if (amount is null)
            return;

        walletService.DepositToWallet(playerId.Value, currency.Value, amount.Value);
        Console.WriteLine("Deposit successful.");
    }

    void WithdrawFromWallet()
    {
        var playerId = ConsolePrompts.PromptPlayerId();
        if (playerId is null)
            return;

        var currency = ConsolePrompts.PromptCurrency();
        if (currency is null)
            return;

        var amount = ConsolePrompts.PromptAmount("Amount to withdraw");
        if (amount is null)
            return;

        walletService.WithdrawFromWallet(playerId.Value, currency.Value, amount.Value);

        Console.WriteLine("Withdrawal successful.");
    }

    void BlockWallet()
    {
        var playerId = ConsolePrompts.PromptPlayerId();
        if (playerId is null)
            return;

        var currency = ConsolePrompts.PromptCurrency();
        if (currency is null)
            return;

        walletService.BlockWallet(playerId.Value, currency.Value);
        Console.WriteLine("Wallet blocked.");
    }

    void UnblockWallet()
    {
        var playerId = ConsolePrompts.PromptPlayerId();
        if (playerId is null)
            return;

        var currency = ConsolePrompts.PromptCurrency();
        if (currency is null)
            return;

        walletService.UnblockWallet(playerId.Value,currency.Value);
        Console.WriteLine("Wallet unblocked.");
    }

    void UpdateWalletBalance()
    {
        var playerId = ConsolePrompts.PromptPlayerId();
        if (playerId is null)
            return;

        var currency = ConsolePrompts.PromptCurrency();
        if (currency is null)
            return;

        var newBalance = ConsolePrompts.PromptAmount("New balance");
        if (newBalance is null)
            return;

        walletService.UpdateWalletBalance(playerId.Value, currency.Value, newBalance.Value);

        Console.WriteLine("Balance updated successfully.");
    }

    void ApplyFundsOperation()
    {
        var playerId = ConsolePrompts.PromptPlayerId();

        if (playerId is null)
            return;

        var currency = ConsolePrompts.PromptCurrency();

        if (currency is null)
            return;

        var operation = ConsolePrompts.PromptFundsOperation();

        if (operation is null)
            return;

        var amount = ConsolePrompts.PromptAmount("Amount");

        if (amount is null)
            return;

        walletService.ApplyFundsOperation(playerId.Value, currency.Value, amount.Value, operation.Value);

        Console.WriteLine("Funds operation applied successfully.");
    }

}
