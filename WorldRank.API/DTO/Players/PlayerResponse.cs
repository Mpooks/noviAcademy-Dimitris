using WorldRank.Domain.Entities.Player;

namespace WorldRank.API.DTO.Players;

public record PlayerResponse(
    int Id,
    string Name,
    int Score)
{
    public static PlayerResponse FromPlayer(
        Player player)
    {
        return new PlayerResponse(player.Id,player.Name,player.Score);
    }
}