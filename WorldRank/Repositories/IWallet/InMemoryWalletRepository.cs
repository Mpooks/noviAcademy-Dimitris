namespace WorldRank
{
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

            player?.Wallets.Add(wallet.Currency, wallet);

        }    

        public List<Wallet> GetByPlayer(int playerId)
        {
            var wallets = _players
                .Where(w => w.Id == playerId)
                .SelectMany(item => item.Wallets.Values);

            return wallets.ToList();
        }
    }
}
