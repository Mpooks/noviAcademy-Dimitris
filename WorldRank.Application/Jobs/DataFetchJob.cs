using MediatR;
using Quartz;
using WorldRank.Application.Interfaces;

namespace WorldRank.Application.Jobs
{
    [DisallowConcurrentExecution]
    public class DataFetchJob : IJob
    {
        private readonly IEcbHttpClient _ecbHttpClient;
        private readonly ISender _sender;
        

        public DataFetchJob(IEcbHttpClient ecbHttpClient, ISender sender)
        {
            _ecbHttpClient = ecbHttpClient;
            _sender = sender;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var dtoResults = await _ecbHttpClient.GetLatestRatesAsync(context.CancellationToken);

        }
    }
}
