using Microsoft.Extensions.DependencyInjection;
using WorldRank.Application.Interfaces;
using WorldRank.Application.Strategies;


namespace WorldRank
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddSingleton<IFundsStrategy, AddFundsStrategy>();
            services.AddSingleton<IFundsStrategy, SubtractFundsStrategy>();
            services.AddSingleton<IFundsStrategy, ForceSubtractFundsStrategy>();

            return services;
        }
    }
}
