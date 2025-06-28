namespace Tournament.Api.Application.Dtos.Tournaments
{
    public record CreateTournamentDto
    {
        public string Name { get; set; }
        public int Count { get; set; }
    }
}
