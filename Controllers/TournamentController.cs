using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tournament.Api.Application.Dtos;
using Tournament.Api.Application.Services;

namespace Tournament.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TournamentController : ControllerBase
    {
        private readonly ITournamentService _tournamentService;
        public TournamentController(ITournamentService tournamentService)
        {
            _tournamentService = tournamentService;
        }


        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody]CreateTournamentDto tournament, CancellationToken cancellationToken) 
        {

            return Ok(await _tournamentService.CreateTournamentAsync(tournament, cancellationToken));
        }
    }
}
