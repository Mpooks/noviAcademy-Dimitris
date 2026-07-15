using MediatR;
using WorldRank.Application.Infrastructure;
using WorldRank.Domain.Entities.Player;

namespace WorldRank.Application.Queries.Players
{
    public class GetPlayerByIdQueryHandler : IRequestHandler<GetPlayerByIdQuery, Player?>
    {
        private readonly IGetPlayerByIdPersistence _persistence;

        public GetPlayerByIdQueryHandler(IGetPlayerByIdPersistence persistence)
        {
            _persistence = persistence;
        }
        public async Task<Player?> Handle(GetPlayerByIdQuery req,CancellationToken cancellationToken)
        {
            return await _persistence.GetByIdAsync(req.playerId, cancellationToken);
        }
    }
}
