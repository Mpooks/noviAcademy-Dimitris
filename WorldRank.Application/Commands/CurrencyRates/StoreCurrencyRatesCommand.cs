using MediatR;
using CurrencyRateE = WorldRank.Domain.Entities.CurrencyRates.CurrencyRates;

namespace WorldRank.Application.Commands.CurrencyRates;

public record StoreCurrencyRatesCommand(IReadOnlyCollection<CurrencyRateE> Rates) : IRequest<int>;