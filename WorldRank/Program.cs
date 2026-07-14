using Microsoft.Extensions.DependencyInjection;
using NLog;
using WorldRank;
using WorldRank.Application.Services;
using WorldRank.Domain.Entities.Wallets;

var logger = LogManager.GetCurrentClassLogger();

var services = new ServiceCollection();
services.AddWorldRank();

using var serviceProvider = services.BuildServiceProvider();
using var scope = serviceProvider.CreateScope();
var playerService = scope.ServiceProvider.GetRequiredService<PlayerService>();
var walletService = scope.ServiceProvider.GetRequiredService<WalletService>();


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

	Func<Task>? action = Console.ReadLine() switch
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
		_ => () => { 
            Console.WriteLine("Unknown option.");
            return Task.CompletedTask;
        }
	};

	if (action is null)
	{
		logger.Info("Application exiting.");
		LogManager.Shutdown(); // flush file writes before exit
		return; // "0" selected — exit
	}

	try
	{
		await action();
	}
	catch (Exception ex)
	{
		// Safety net: log any exception the specific handlers did not catch, and keep the app running.
		logger.Error(ex, "Unexpected error while handling a menu action");
		Console.WriteLine($"Unexpected error: {ex.Message}");
	}

    async Task AddPlayer()
    {
        var name = ConsolePrompts.PromptName();
        var score = ConsolePrompts.PromptScore();

        if (name is null || score is null)
            return;

        await playerService.AddPlayerAsync(name, score.Value, CancellationToken.None);

        Console.WriteLine("Player added successfully.");
    }

    async Task ListPlayers()
    {
        var players = await playerService.ListPlayersAsync(CancellationToken.None);

        if (players.Count == 0)
        {
            Console.WriteLine(
                "No players registered.");

            return;
        }

        foreach (var player in players)
            Console.WriteLine(player);
    }

    async Task ListPlayersByScore()
    {
        var groups =await playerService.ListPlayersByScoreAsync(CancellationToken.None);

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

    async Task FindPlayerByName()
    {
        var name = ConsolePrompts.PromptName();
        if (name is null)
            return;

        var player = await playerService.FindPlayerByNameAsync(name, CancellationToken.None);

        if (player is null)
        {
            Console.WriteLine("No player found.");
            return;
        }

        Console.WriteLine(player);
    }

    async Task FindPlayerById()
    {
        var playerId =
            ConsolePrompts.PromptPlayerId();

        if (playerId is null)
            return;

        var player = await playerService.FindPlayerByIdAsync(playerId.Value, CancellationToken.None);

        if (player is null)
        {
            Console.WriteLine("No player found.");
            return;
        }

        Console.WriteLine(player);
    }

    async Task DeletePlayer()
    {
        var playerId =ConsolePrompts.PromptPlayerId();

        if (playerId is null)
            return;

        await playerService.DeletePlayerAsync(playerId.Value,CancellationToken.None);

        Console.WriteLine("Player deleted if it existed.");
    }

    async Task AddWalletToPlayer()
    {
        var playerId =ConsolePrompts.PromptPlayerId();

        if (playerId is null)
            return;

        var currency =ConsolePrompts.PromptCurrency();

        if (currency is null)
            return;

        var balance =ConsolePrompts.PromptAmount("Initial balance");

        if (balance is null)
            return;

        await walletService.AddWalletToPlayerAsync(
            playerId.Value,
            currency.Value,
            balance.Value,
            CancellationToken.None);

        Console.WriteLine(
            "Wallet added successfully.");
    }

    async Task GetWalletsOfPlayer()
    {
        var playerId = ConsolePrompts.PromptPlayerId();
        if (playerId is null)
            return;

        var wallets = await walletService.GetWalletsOfPlayerAsync(playerId.Value, CancellationToken.None);
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

    async Task DepositToWallet()
    {
        var walletId = ConsolePrompts.PromptWalletId();

        if (walletId is null)
            return;

        var amount = ConsolePrompts.PromptAmount(
            "Amount to deposit");

        if (amount is null)
            return;

        var wallet = await walletService.DepositToWalletAsync(
            walletId.Value,
            amount.Value,
            CancellationToken.None);

        if (wallet is null)
        {
            Console.WriteLine("Wallet not found.");
            return;
        }

        Console.WriteLine(
            $"Deposit successful. New balance: {wallet.Balance}");
    }

    async Task WithdrawFromWallet()
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

        await walletService.WithdrawFromWalletAsync(playerId.Value, currency.Value, amount.Value, CancellationToken.None);

        Console.WriteLine("Withdrawal successful.");
    }

    async Task BlockWallet()
    {
        var playerId = ConsolePrompts.PromptPlayerId();
        if (playerId is null)
            return;

        var currency = ConsolePrompts.PromptCurrency();
        if (currency is null)
            return;

        await walletService.BlockWalletAsync(playerId.Value, currency.Value, CancellationToken.None);
        Console.WriteLine("Wallet blocked.");
    }

    async Task UnblockWallet()
    {
        var playerId = ConsolePrompts.PromptPlayerId();
        if (playerId is null)
            return;

        var currency = ConsolePrompts.PromptCurrency();
        if (currency is null)
            return;

        await walletService.UnblockWalletAsync(playerId.Value, currency.Value, CancellationToken.None);
        Console.WriteLine("Wallet unblocked.");
    }

    async Task UpdateWalletBalance()
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

        await walletService.UpdateWalletBalanceAsync(playerId.Value, currency.Value, newBalance.Value, CancellationToken.None);

        Console.WriteLine("Balance updated successfully.");
    }

    async Task ApplyFundsOperation()
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

        await walletService.ApplyFundsOperationAsync(playerId.Value, currency.Value, amount.Value, operation.Value, CancellationToken.None);

        Console.WriteLine("Funds operation applied successfully.");
    }

}
