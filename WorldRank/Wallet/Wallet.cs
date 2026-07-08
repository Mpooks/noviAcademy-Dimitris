namespace WorldRank;

public class Wallet
{
    public decimal Balance { get; private set; }
    public Currency Currency { get; }
    public bool IsBlocked { get; private set; }

    public Wallet(Currency currency)
    {
        Balance = 0;
        Currency = currency;
        IsBlocked = false;
    }

    public void Deposit(decimal amount)
    {
        if (IsBlocked)
        {
            throw new InvalidOperationException("The wallet is blocked.");
        }

        if (amount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), "The amount to be deposited should be positive.");
        }

        Balance += amount;
    }

    public void Withdraw(decimal amount)
    {
        if (IsBlocked)
        {
            throw new InvalidOperationException("The wallet is blocked.");
        }

        if (amount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), "The amount to be withdrawn needs to be positive.");
        }

        if (amount > Balance)
        {
            throw new InvalidOperationException("There are insufficient funds for the transaction to happen.");
        }

        Balance -= amount;
    }

    public void ToggleBlock()
    {
        IsBlocked = !IsBlocked;
    }

    public override string ToString()
    {
        string status = IsBlocked ? "Blocked" : "Not blocked";
        return $"Status = {status} | Balance = {Balance} {Currency}";
    }
}