using MediatR;
using Microsoft.AspNetCore.Mvc;
using WorldRank.API.DTO.Wallets;
using WorldRank.Application.Commands.Wallets;
using WorldRank.Application.Queries.Wallets;
using WorldRank.Application.Services;
using WorldRank.Domain.Entities.Exceptions;

namespace WorldRank.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WalletsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public WalletsController(IMediator mediator)
        {

            _mediator = mediator;
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetWalletById(int id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetWalletByIdQuery(id), cancellationToken);
            if (result is null)
                return NotFound();
            var response = WalletResponse.FromWallet(result);

            return Ok(response);

        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateWalletRequest req, CancellationToken cancellationToken)
        {
            try {
                var walletId = await _mediator.Send(new CreateWalletCommand(req.PlayerId, req.Currency), cancellationToken);

                var response = new WalletResponse(walletId, req.PlayerId, req.Currency.ToString(), 0m, false);

                return CreatedAtAction(nameof(GetWalletById), new { id = walletId }, response);
            }
            catch (PlayerNotFoundException exception)
            {
                return NotFound(exception.Message);
            }
            catch (DuplicateWalletException exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpPost("{id:int}/deposit")]
        public async Task<IActionResult> Deposit([FromRoute] int id,[FromBody] DepositRequest req, CancellationToken cancellationToken)
        {
            try {
                var wallet = await _mediator.Send(new DepositToWalletCommand(id, req.Amount), cancellationToken);
                if (wallet is null)
                {
                    return NotFound();
                }
                var response = WalletResponse.FromWallet(wallet);
                return Ok(response);
            }
            catch (WalletException exception)
            {
                return BadRequest(exception.Message);
            }
        }
    }
}

