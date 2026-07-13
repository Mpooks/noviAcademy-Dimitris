using WorldRank.Domain.Entities.Wallets;

namespace WorldRank.API.DTO.Wallets
{
    public record WalletResponse(int Id,int PlayerId,string Currency ,decimal Balance,bool IsBlocked)
    {
        public static WalletResponse FromWallet(Wallet wallet)
        {
            return new WalletResponse(wallet.Id,wallet.PlayerId,wallet.Currency.ToString(),wallet.Balance,wallet.IsBlocked);
        }
    }
}
