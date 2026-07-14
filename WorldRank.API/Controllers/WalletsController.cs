using Microsoft.AspNetCore.Mvc;
using WorldRank.API.DTO.Wallets;
using WorldRank.Application.Services;
using WorldRank.Domain.Entities.Exceptions;

namespace WorldRank.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WalletsController : ControllerBase
    {
        private readonly WalletService _walletService;

        public WalletsController(WalletService walletService)
        {
            _walletService = walletService;
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetWalletById(int id, CancellationToken cancellationToken)
        {
            var result = await  _walletService.GetWalletByIdAsync(id, cancellationToken);
            if (result is null)
                return NotFound();
            var response = WalletResponse.FromWallet(result);

            return Ok(response);

        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateWalletRequest req, CancellationToken cancellationToken)
        {
            var wallet = await _walletService.AddWalletToPlayerAsync(req.PlayerId, req.Currency, 0m, cancellationToken);
            var response = WalletResponse.FromWallet(wallet);

            return CreatedAtAction(nameof(GetWalletById), new { id = wallet.Id }, response);
        }

        [HttpPost("{id:int}/deposit")]
        public async Task<IActionResult> Deposit([FromRoute] int id,[FromBody] DepositRequest req, CancellationToken cancellationToken)
        {
            try {
                var wallet = await _walletService.DepositToWalletAsync(id, req.Amount, cancellationToken);
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

