using MediatR;
using Quartz;
using WorldRank.Gateway.Clients;
using WorldRank.Application.Commands.CurrencyRates;
using CurrencyRateE = WorldRank.Domain.Entities.CurrencyRates.CurrencyRates;

namespace WorldRank.API.Jobs
{
    [DisallowConcurrentExecution]
    public class UpdateCurrencyRatesJob : IJob
    {
        private readonly IEcbHttpClient _ecbHttpClient;
        private readonly ISender _sender;
        

        public UpdateCurrencyRatesJob(IEcbHttpClient ecbHttpClient, ISender sender)
        {
            _ecbHttpClient = ecbHttpClient;
            _sender = sender;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var dtoResults = await _ecbHttpClient.GetLatestRatesAsync(context.CancellationToken);

            var currencyRates = dtoResults.Select(ditto => new CurrencyRateE(ditto.Currency, ditto.Rate, ditto.Date)).ToArray();

            await _sender.Send(new StoreCurrencyRatesCommand(currencyRates), context.CancellationToken);
        }
    }
}
