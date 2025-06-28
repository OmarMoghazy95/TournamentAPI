using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tournament.Api.Application.Dtos.Tournaments;
using Tournament.Api.Application.Services;

namespace Tournament.Api.Presentation.Controllers;


public class TournamentController : BaseController
{
    private readonly ITournamentService _tournamentService;
    public TournamentController(ITournamentService tournamentService)
    {
        _tournamentService = tournamentService;
    }


    [HttpPost("")]
    public async Task<IActionResult> Create([FromBody] CreateTournamentDto tournament, CancellationToken cancellationToken)
    {

        return HandleResponse(await _tournamentService.CreateTournamentAsync(tournament, cancellationToken));
    }

    [HttpGet("{tournamentId}")]
    public async Task<IActionResult> GetTournamentById(Guid tournamentId, CancellationToken cancellationToken)
    {
        return HandleResponse(await _tournamentService.GetTournamentById(tournamentId, cancellationToken));
    }
}
