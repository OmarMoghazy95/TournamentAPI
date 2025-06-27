namespace Tournament.Api.Core.Entities
{
    internal class TournamentTeam : BaseEntity
    {
        public Guid TournamentId { get; set; }
        public string TeamName { get; set; }

        public virtual Tournament Tournament { get; set; }
    }
}
