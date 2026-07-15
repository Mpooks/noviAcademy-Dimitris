using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WorldRank.Application.Infrastructure;
using WorldRank.Application.Interfaces;
using WorldRank.Domain.Entities.Player;
using WorldRank.Infrastructure.Data;

namespace WorldRank.Infrastructure.Persistence.Queries
{
    public class GetAllPersistence : IGetAllPersistence
    {
        private readonly WorldRankDbContext _dbContext;

        public GetAllPersistence(WorldRankDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Player>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _dbContext.Players.AsNoTracking().ToListAsync(cancellationToken);
        }
    }
}
