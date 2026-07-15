using MediatR;
using WorldRank.Application.Infrastructure;

namespace WorldRank.Application.Commands.CurrencyRates
{
    public class StoreCurrencyRatesCommandHandler : IRequestHandler<StoreCurrencyRatesCommand, int>
    {
        private IStoreCurrencyRatesPersistence _persistence;

        public StoreCurrencyRatesCommandHandler(IStoreCurrencyRatesPersistence persistence)
        {
            _persistence = persistence;
        }

        public Task<int> Handle(StoreCurrencyRatesCommand request, CancellationToken cancellationToken)
        {
            return _persistence.Persist(request.Rates, cancellationToken);
        }
    }
}
