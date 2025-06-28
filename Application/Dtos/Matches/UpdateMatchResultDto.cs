using Tournament.Api.Application.Enums;

namespace Tournament.Api.Application.Dtos.Matches
{
    public record UpdateMatchResultDto
    {
        public MatchResult Result { get; set; }
        public Guid? WinningTeamId { get; set; }
    }
}
