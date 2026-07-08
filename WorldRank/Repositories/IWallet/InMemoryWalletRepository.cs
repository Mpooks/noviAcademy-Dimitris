namespace WorldRank;

public class InMemoryWalletRepository : IWalletRepository
{
    private readonly List<Player> _players;

    public InMemoryWalletRepository(List<Player> players)
    {
        _players = players;
    }

    public void Add(Wallet wallet, int playerId)
    {
        if (wallet == null)
        {
            throw new ArgumentNullException(nameof(wallet));
        }

        var player = _players.FirstOrDefault(p => p.Id == playerId);

        if (player == null)
        {
            throw new InvalidOperationException("Player does not exist");
        }

        if (player.Wallets.ContainsKey(wallet.Currency))
        {
            throw new InvalidOperationException("Player already has a wallet of that currency");
        }

        player?.AddWallet(wallet);
    }

    public List<Wallet> GetByPlayer(int playerId)
    {
        var wallets = _players
            .Where(p => p.Id == playerId)
            .SelectMany(p => p.Wallets.Values);

        return wallets.ToList();
    }
}