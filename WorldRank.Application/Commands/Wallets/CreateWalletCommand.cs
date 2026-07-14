using MediatR;
using WorldRank.Domain.Entities.Enums;
namespace WorldRank.Application.Commands.Wallets;

public record CreateWalletCommand(int PlayerId, Currency Currency) : IRequest<int>;
