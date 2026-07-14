using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using WorldRank.Infrastructure.Data;
using WorldRank.Application.Interfaces;
using WorldRank.Infrastructure.Repositories;


namespace WorldRank.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        var connectionString =
            "Server=localhost;" +
            "Database=WorldRank;" +
            "Integrated Security=true;" +
            "TrustServerCertificate=true;";

        services.AddDbContext<WorldRankDbContext>(options => options.UseSqlServer(connectionString));
        services.AddScoped<IPlayerRepository, DBPlayerRepository>();
        services.AddScoped<IWalletRepository, DBWalletRepository>();

        return services;
    }
}