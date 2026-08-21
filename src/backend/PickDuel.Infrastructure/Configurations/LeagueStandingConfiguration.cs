using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PickDuel.Domain.Entities.Standings;

namespace PickDuel.Infrastructure.Configurations;

public class LeagueStandingConfiguration :
    IEntityTypeConfiguration<LeagueStanding>
{
    /// <summary>
    /// Configures league standing persistence.
    /// </summary>
    public void Configure(
        EntityTypeBuilder<LeagueStanding> builder)
    {
        builder.HasKey(x => x.Id);


        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey("UserId");


        builder.HasOne(x => x.League)
            .WithMany()
            .HasForeignKey("LeagueId");


        builder.Property(x => x.TotalPoints)
            .IsRequired();
    }
}