using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tournament.Api.Core.Entities;

namespace Tournament.Api.Infrastructure.Configurations
{
    internal class TournamentMatchConfiguration : BaseConfiguration<TournamentMatch>
    {
        public override void Configure(EntityTypeBuilder<TournamentMatch> builder)
        {
            base.Configure(builder);

            builder.HasOne(a => a.ParentMatchA).WithOne(a=>a.ChildMatchA)
                .HasForeignKey<TournamentMatch>(a => a.MatchAId).IsRequired(false)
                .OnDelete(Microsoft.EntityFrameworkCore.DeleteBehavior.NoAction);

            builder.HasOne(a => a.ParentMatchB).WithOne(a=>a.ChildMatchB)
               .HasForeignKey<TournamentMatch>(a => a.MatchBId).IsRequired(false)
               .OnDelete(Microsoft.EntityFrameworkCore.DeleteBehavior.NoAction);

            

            builder.HasOne(a => a.Winner).WithMany(a => a.MatchesWon).HasForeignKey(a => a.WinnerId).IsRequired(false);
            builder.HasOne(a => a.TeamA).WithMany(a => a.MatchesA).HasForeignKey(a => a.TeamAId).IsRequired(false);
            builder.HasOne(a => a.TeamB).WithMany(a => a.MatchesB).HasForeignKey(a => a.TeamBId).IsRequired(false);
        }
    }
}
