using MediatR;
using WorldRank.Domain.Entities.Player;

namespace WorldRank.Application.Queries.Players
{
    public record GetPlayerByIdQuery(int playerId) : IRequest<Player?>;
}
