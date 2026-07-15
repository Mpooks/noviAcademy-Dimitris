using WorldRank.Application.Interfaces;
using WorldRank.Application.Services;
using WorldRank.Application.Strategies;
using WorldRank.Application;
using WorldRank.Infrastructure.Caching;
using WorldRank.Infrastructure.Data;
using WorldRank.Infrastructure.Repositories;
using WorldRank.Infrastructure;
using Autofac.Extensions.DependencyInjection;
using Autofac;
using Microsoft.EntityFrameworkCore;
using NLog.Extensions.Logging;
using System.Text.Json.Serialization;
using Quartz;
using WorldRank.API.Jobs;
using WorldRank.Gateway.Clients;
using WorldRank.Gateway;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Host.ConfigureContainer<ContainerBuilder>(container =>
{
    container.RegisterModule(new ApplicationModule());
    container.RegisterModule(new InfrastructureModule());
});

// Logging
builder.Logging.ClearProviders();
builder.Logging.AddNLog("nlog.config");

// DbContext
builder.Services.AddDbContext<WorldRankDbContext>(options =>
{
    options.UseSqlServer("Server=localhost;Database=WorldRank;Integrated Security=true;TrustServerCertificate=true;");
});

// Repositories
builder.Services.AddScoped<IPlayerRepository, DBPlayerRepository>();
builder.Services.AddScoped<IWalletRepository, DBWalletRepository>();

// Services
builder.Services.AddScoped<PlayerService>();
builder.Services.AddScoped<WalletService>();

//Quartz & HttpClient
builder.Services.AddGateway();
builder.Services.AddQuartz(q =>
{
    var jobKey = new JobKey(nameof(UpdateCurrencyRatesJob));
    q.AddJob<UpdateCurrencyRatesJob>(jobKey);
    q.AddTrigger(t => t
    .ForJob(jobKey)
    .WithIdentity($"{nameof(UpdateCurrencyRatesJob)}-trigger")
    .WithCronSchedule("0/5 * * * * ?"));
});

builder.Services.AddQuartzHostedService();

// In-memory cache
builder.Services.AddMemoryCache();

builder.Services.AddSingleton<ICache, MemoryCacheStore>();
builder.Services.AddSingleton<IFundsStrategy, AddFundsStrategy>();
builder.Services.AddSingleton<IFundsStrategy, SubtractFundsStrategy>();
builder.Services.AddSingleton<IFundsStrategy, ForceSubtractFundsStrategy>();

builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(
            new JsonStringEnumConverter());
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapGet("/", () => Results.Redirect("/swagger"));
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
