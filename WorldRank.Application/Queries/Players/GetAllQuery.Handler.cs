using MediatR;
using WorldRank.Application.Infrastructure;
using WorldRank.Domain.Entities.Player;


namespace WorldRank.Application.Queries.Players
{
    public class GetAllQueryHandler : IRequestHandler<GetAllQuery, List<Player>>
    {
        private readonly IGetAllPersistence _persistence;
        public GetAllQueryHandler(IGetAllPersistence persistence)
        {
            _persistence = persistence;
        }

        public async Task<List<Player>> Handle(GetAllQuery req, CancellationToken cancellationToken)
        {
            return await _persistence.GetAllAsync(cancellationToken);
        }
    }
}
