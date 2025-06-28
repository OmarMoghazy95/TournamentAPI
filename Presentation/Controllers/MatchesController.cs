using Microsoft.AspNetCore.Mvc;
using Tournament.Api.Application.Dtos.Matches;
using Tournament.Api.Application.Services;

namespace Tournament.Api.Presentation.Controllers
{
    public class MatchesController : BaseController
    {
        private readonly IMatchService _matchService;

        public MatchesController(IMatchService matchService)
        {
            _matchService = matchService;
        }


        [HttpPost("{matchId}/finish")]

        public async Task<IActionResult> Finish([FromRoute] Guid matchId, [FromBody] UpdateMatchResultDto status, CancellationToken cancellationToken)
        {
            return HandleResponse(await _matchService.FinishMatch(matchId, status));
        }
    }
}
