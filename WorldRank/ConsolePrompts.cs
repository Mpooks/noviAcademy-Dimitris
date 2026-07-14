using WorldRank.Application.Strategies;
using WorldRank.Domain.Entities.Enums;

namespace WorldRank
{
    public static class ConsolePrompts
    {
        public static string? PromptName()
        {
            Console.Write("Name: ");
            var name = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(name))
                return name;

            Console.WriteLine("Name cannot be empty.");
            return null;
        }

        public static int? PromptScore()
        {
            Console.Write("Score: ");

            if (int.TryParse(Console.ReadLine(), out var score))
                return score;

            Console.WriteLine("Score must be a whole number.");
            return null;
        }

        public static int? PromptPlayerId()
        {
            Console.Write("Give player id: ");
            if (int.TryParse(Console.ReadLine(), out var playerId))
                return playerId;

            Console.WriteLine("Player id must be a whole number.");
            return null;
        }

        public static int? PromptWalletId()
        {
            Console.Write("Give wallet id: ");

            if (int.TryParse(Console.ReadLine(),out var walletId))
            {
                return walletId;
            }

            Console.WriteLine("Wallet id must be a whole number.");

            return null;
        }

        public static Currency? PromptCurrency()
        {
            Console.Write("Give Currency: 1 - EUR | 2 - USD\n");
            switch (Console.ReadLine())
            {
                case "1":
                    return Currency.EUR;
                case "2":
                    return Currency.USD;
                default:
                    Console.WriteLine("Unknown currency.");
                    return null;
            }
        }

        public static decimal? PromptAmount(string label)
        {
            Console.Write($"{label}: ");
            if (decimal.TryParse(Console.ReadLine(), out var amount))
                return amount;

            Console.WriteLine("Amount must be a number.");
            return null;
        }

        public static FundsOperation? PromptFundsOperation()
        {
            Console.WriteLine("Choose an operation: 1-Add | 2-Subtract | 3-Force Subtract (balance may become negative)");

            var operation = Console.ReadLine() switch
            {
                "1" => FundsOperation.Add,
                "2" => FundsOperation.Subtract,
                "3" => FundsOperation.ForceSubtract,
                _ => (FundsOperation?)null
            };

            if (operation is null)
            {
                Console.WriteLine(
                    "Unknown funds operation.");
            }

            return operation;
        }

    }
}
