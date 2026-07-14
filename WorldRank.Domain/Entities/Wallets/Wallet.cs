using WorldRank.Domain.Entities.Exceptions;
using WorldRank.Domain.Entities.Enums;

namespace WorldRank.Domain.Entities.Wallets
{
	public class Wallet : IWallet
	{
		public int Id { get; private set; }
		public Currency Currency { get; private set; }
		public int PlayerId { get; private set; }
		public decimal Balance { get; private set; }
		public bool IsBlocked { get; private set; }

		private Wallet()
		{
		}

		public Wallet(int playerId, Currency currency, decimal balance, bool isBlocked = false)
		{
			PlayerId = playerId;
			if (balance < 0)
				throw new InsufficientFundsException(balance);

			Balance = balance;
			Currency = currency;
			IsBlocked = isBlocked;
		}

		public void Block() => IsBlocked = true;

		public void Unblock() => IsBlocked = false;

		public void SetBalance(decimal balance)
		{
			if (balance < 0)
				throw new InsufficientFundsException(balance);

			Balance = balance;
		}

		public void Deposit(decimal amount)
		{
			if (amount <= 0)
				throw new InvalidAmountException(amount);

			if (IsBlocked)
				throw new WalletBlockedException(Currency);

			Balance += amount;
		}

		public void Withdraw(decimal amount)
		{
			if (amount <= 0)
				throw new InvalidAmountException(amount);

			if (IsBlocked)
				throw new WalletBlockedException(Currency);

			var newBalance = Balance - amount;
			if (newBalance < 0)
				throw new InsufficientFundsException(newBalance);

			Balance = newBalance;
		}

		public void ForceSubtractFunds(decimal amount)
		{

            if (IsBlocked)
                throw new WalletBlockedException(Currency);

			Balance -= amount;
        }

		public override string ToString() => $"Balance -> {Balance} Currency -> {Currency} IsBlocked -> {IsBlocked}";
	}
}
