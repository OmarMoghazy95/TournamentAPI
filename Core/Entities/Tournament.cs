namespace Tournament.Api.Core.Entities
{
    public class Tournament : BaseEntity
    {
        public string Name { get; set; }
        public int TeamCount { get; set; }

        public virtual ICollection<TournamentMatch> Matches { get; set; } = new HashSet<TournamentMatch>();
        public virtual ICollection<TournamentTeam> Teams { get; set; } = new HashSet<TournamentTeam>();

    }
}
