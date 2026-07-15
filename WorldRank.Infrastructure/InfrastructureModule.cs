using Autofac;
using WorldRank.Application.Infrastructure;
using WorldRank.Infrastructure.Persistence.Commands.Players;
using WorldRank.Infrastructure.Persistence.Commands.Wallets;
using WorldRank.Infrastructure.Persistence.Queries;


namespace WorldRank.Infrastructure
{
    public class InfrastructureModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CreatePlayerPersistence>().As<ICreatePlayerPersistence>().InstancePerLifetimeScope();
            builder.RegisterDecorator<CreatePlayerPersistenceCachingDecorator, ICreatePlayerPersistence>();
            builder.RegisterType<GetPlayerByIdPersistence>().As<IGetPlayerByIdPersistence>().InstancePerLifetimeScope();
            builder.RegisterDecorator<GetPlayerByIdPersistenceCachingDecorator, IGetPlayerByIdPersistence>();
            builder.RegisterType<GetAllPersistence>().As<IGetAllPersistence>().InstancePerLifetimeScope();
            builder.RegisterDecorator<GetAllPersistenceCachingDecorator, IGetAllPersistence>();
            builder.RegisterType<CreateWalletPersistence>().As<ICreateWalletPersistence>().InstancePerLifetimeScope();
            builder.RegisterDecorator<CreateWalletPersistenceCachingDecorator, ICreateWalletPersistence>();
            builder.RegisterType<DepositToWalletPersistence>().As<IDepositToWalletPersistence>().InstancePerLifetimeScope();
            builder.RegisterDecorator<DepositToWalletPersistenceCachingDecorator, IDepositToWalletPersistence>();
            builder.RegisterType<GetWalletByIdPersistence>().As<IGetWalletByIdPersistence>().InstancePerLifetimeScope();
            builder.RegisterDecorator<GetWalletByIdPersistenceCachingDecorator, IGetWalletByIdPersistence>();
        }
    }
}
