using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tournament.Api.Core.Entities;

namespace Tournament.Api.Infrastructure.Configurations
{
    internal class TournamentConfiguration : BaseConfiguration<Core.Entities.Tournament>
    {
        public override void Configure(EntityTypeBuilder<Core.Entities.Tournament> builder)
        {
            base.Configure(builder);

            builder.HasMany(a => a.Teams).WithOne(a => a.Tournament).HasForeignKey(a => a.TournamentId)
                .OnDelete(Microsoft.EntityFrameworkCore.DeleteBehavior.NoAction);

            builder.Property(a => a.Name).HasMaxLength(255);

            builder.HasMany(a => a.Matches).WithOne(a => a.Tournament).HasForeignKey(a => a.TournamentId);

        }
    }
}
