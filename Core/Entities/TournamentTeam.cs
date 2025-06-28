namespace Tournament.Api.Core.Entities
{
    internal class TournamentTeam : BaseEntity
    {
        public Guid TournamentId { get; set; }
        public string TeamName { get; set; }

        public virtual Tournament Tournament { get; set; }

        public virtual ICollection<TournamentMatch> MatchesA { get; set; }
        public virtual ICollection<TournamentMatch> MatchesB { get; set; }
        public virtual ICollection<TournamentMatch> MatchesWon { get; set; }


    }
}
