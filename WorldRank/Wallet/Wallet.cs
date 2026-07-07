using System.Xml.Linq;
using WorldRank;

public class Wallet
{
    public decimal Balance { get; private set; }
    public Currency Currency;
    public bool IsBlocked;

    public Wallet(decimal balance, Currency currency, bool isBlocked)
    {
        Balance = balance;
        Currency = currency;
        IsBlocked = false;
    }

    public void SetBalance(decimal balance)
    {
        if (balance < 0) { return; }
        Balance = balance;
    }

    public override string ToString()
    {
        string status = IsBlocked ? "Blocked" : "Not blocked";
        return $"Status = {status} | Balance = {Balance} {Currency}";
    }
}