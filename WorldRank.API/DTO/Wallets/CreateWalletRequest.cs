using WorldRank.Domain.Entities.Enums;

namespace WorldRank.API.DTO.Wallets
{
    public record CreateWalletRequest(int PlayerId, Currency Currency);
    public record DepositRequest(decimal Amount);

}
