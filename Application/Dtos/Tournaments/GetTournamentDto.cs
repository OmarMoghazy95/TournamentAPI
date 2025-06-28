using Tournament.Api.Core.Enums;

namespace Tournament.Api.Application.Dtos.Tournaments
{
    public record GetTournamentDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<GetTournamentMatchDto> Matches { get; set; }
    }

    public record GetTournamentMatchDto
    {
        public Guid Id { get; set; }
        public string TeamA { get; set; }
        public string TeamB { get; set; }
        public string Winner { get; set; }
        public uint Round { get; set; }
        public MatchState Status { get; set; }

    }
}
