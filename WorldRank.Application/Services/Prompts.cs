using WorldRank.Application.Strategies;
using WorldRank.Domain.Entities.Enums;

namespace WorldRank.Application.Services
{
    public static class Prompts
    {
        public static int? PromptPlayerId()
        {
            Console.Write("Give player id: ");
            if (int.TryParse(Console.ReadLine(), out var playerId))
                return playerId;

            Console.WriteLine("Player id must be a whole number.");
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

            return Console.ReadLine() switch
            {
                "1" => FundsOperation.Add,
                "2" => FundsOperation.Subtract,
                "3" => FundsOperation.ForceSubtract,
                _ => null
            };
        }

    }
}
