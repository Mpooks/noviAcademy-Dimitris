using Microsoft.Extensions.DependencyInjection;
using WorldRank.Gateway.Clients;

namespace WorldRank.Gateway;

public static class DependencyInjection
{
    public static IServiceCollection AddGateway(this IServiceCollection services)
    {
        services.AddHttpClient<IEcbHttpClient, EcbHTTPClient>();
        return services;
    }
}

