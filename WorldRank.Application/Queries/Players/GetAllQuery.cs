
using MediatR;
using WorldRank.Domain.Entities.Player;

namespace WorldRank.Application.Queries.Players
{
    public record GetAllQuery : IRequest<List<Player>>
    {

    }
}
