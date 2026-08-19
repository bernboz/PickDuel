using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PickDuel.Domain.Entities;

namespace PickDuel.Infrastructure.Configurations;

public class PickConfiguration : IEntityTypeConfiguration<Pick>
{
    /// <summary>
    /// Configures database mapping for prediction picks.
    /// </summary>
    /// <param name="builder">
    /// Entity builder used to configure Pick persistence.
    /// </param>
    public void Configure(
        EntityTypeBuilder<Pick> builder)
    {
        builder.HasKey(x => x.Id);


        builder.Property(x => x.SelectedTeam)
            .IsRequired()
            .HasMaxLength(100);


        builder.Property(x => x.ConfidenceMultiplier)
            .IsRequired();


        builder.Property(x => x.CreatedAt)
            .IsRequired();


        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey("UserId")
            .OnDelete(DeleteBehavior.Cascade);


        builder.HasOne(x => x.League)
            .WithMany()
            .HasForeignKey("LeagueId")
            .OnDelete(DeleteBehavior.Cascade);


        builder.HasOne(x => x.Game)
            .WithMany()
            .HasForeignKey("GameId")
            .OnDelete(DeleteBehavior.Cascade);


        builder.OwnsOne(
            x => x.ScorePrediction,
            prediction =>
            {
                prediction.Property(x => x.HomeScore)
                    .HasColumnName("PredictedHomeScore");

                prediction.Property(x => x.AwayScore)
                    .HasColumnName("PredictedAwayScore");
            });
    }
}