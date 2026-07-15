using Microsoft.AspNetCore.Mvc;
using WorldRank.API.DTO.Players;
using WorldRank.Application.Services;

namespace WorldRank.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlayersController : ControllerBase
    {
        private readonly PlayerService _playerService;

        public PlayersController(PlayerService playerService)
        {
            _playerService = playerService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var players = await _playerService.ListPlayersAsync(cancellationToken);
            var response = players.Select(PlayerResponse.FromPlayer).ToList();

            return Ok(response);
        }

        [HttpGet("{playerId:int}")]
        public async Task<IActionResult> GetPlayerById(int playerId, CancellationToken cancellationToken)
        {
            var result =await  _playerService.FindPlayerByIdAsync(playerId, cancellationToken);
            if (result is null)
                return NotFound();
            var response = PlayerResponse.FromPlayer(result);

            return Ok(response);

        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePlayerRequest req, CancellationToken cancellationToken)
        {
            var id = await _playerService.AddPlayerAsync(req.Name, req.Score, cancellationToken);
            var response = new PlayerResponse(id, req.Name, req.Score);

            return CreatedAtAction(nameof(GetPlayerById), new { playerId = id }, response);
        }
    }
}

