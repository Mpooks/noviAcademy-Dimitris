using System;
using System.Collections.Generic;
using System.Text;
using WorldRank.Domain.Entities.Player;

namespace WorldRank.Application.Infrastructure
{
    public interface IGetAllPersistence
    {
        Task<List<Player>> GetAllAsync(CancellationToken cancellationToken);
    }
}
