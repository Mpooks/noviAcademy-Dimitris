namespace WorldRank
{
    internal interface IWalletRepository
    {
        void Add(Wallet wallet, int playerId);
        List<Wallet> GetByPlayer(int playerId);
    }
}
