using MediatR;
using Microsoft.AspNetCore.Mvc;
using WorldRank.API.DTO.Players;
using WorldRank.Application.Commands.Players;
using WorldRank.Application.Queries.Players;
using WorldRank.Application.Services;

namespace WorldRank.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlayersController : ControllerBase
    {
        private readonly PlayerService _playerService;
        private readonly IMediator _mediator;

        public PlayersController(PlayerService playerService, IMediator mediator)
        {
            _playerService = playerService;
            _mediator = mediator;
        }

        [HttpGet]
        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var players = await _mediator.Send(new GetAllQuery(), cancellationToken);

            var response = players.Select(PlayerResponse.FromPlayer).ToList();

            return Ok(response);
        }

        [HttpGet("{playerId:int}")]
        public async Task<IActionResult> GetPlayerById(int playerId, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetPlayerByIdQuery(playerId), cancellationToken);
            
            if (result is null)
                return NotFound();
            var response = PlayerResponse.FromPlayer(result);

            return Ok(response);

        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePlayerRequest req, CancellationToken cancellationToken)
        {
            int id;
            try
            {
                id = await _mediator.Send(new CreatePlayerCommand(req.Name, req.Score), cancellationToken);
            }catch(ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }

            var response = new PlayerResponse(id, req.Name, req.Score);

            return CreatedAtAction(nameof(GetPlayerById), new { playerId = id }, response);
        }
    }
}

