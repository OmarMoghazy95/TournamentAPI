using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tournament.Api.Core.Entities;

namespace Tournament.Api.Infrastructure.Configurations
{
    internal class TournamentTeamConfigurations : BaseConfiguration<TournamentTeam>
    {
        public override void Configure(EntityTypeBuilder<TournamentTeam> builder)
        {
            base.Configure(builder);
            builder.Property(a => a.TeamName).HasMaxLength(255);

            builder.HasOne(a => a.Tournament).WithMany(a => a.Teams).HasForeignKey(a => a.TournamentId);


        }
    }
}
