using Tournament.Api.Core.Enums;

namespace Tournament.Api.Core.Entities
{
    public class TournamentMatch : BaseEntity
    {
        public Guid TournamentId { get; set; }
        public Guid? TeamAId { get; set; }
        public Guid? TeamBId { get; set; }

        public Guid? WinnerId { get; set; }
        public MatchState Status { get; set; }

        public uint RoundNumber { get; set; }
        public Guid? MatchAId { get; set; }
        public Guid? MatchBId { get; set; }

        public virtual TournamentMatch ParentMatchA { get; set; }
        public virtual TournamentMatch ParentMatchB { get; set; }

        public virtual TournamentMatch ChildMatchA { get; set; }
        public virtual TournamentMatch ChildMatchB { get; set; }


        public virtual TournamentTeam TeamA { get; set; }
        public virtual TournamentTeam TeamB { get; set; }
        public virtual TournamentTeam Winner { get; set; }

        public virtual Tournament Tournament { get; set; }
    }
}
