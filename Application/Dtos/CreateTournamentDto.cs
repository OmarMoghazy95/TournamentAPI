namespace Tournament.Api.Application.Dtos
{
    public record CreateTournamentDto
    {
        public string Name { get; set; }
        public int Count { get; set; }
    }
}
