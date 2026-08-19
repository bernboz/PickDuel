using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PickDuel.Domain.Entities;

namespace PickDuel.Infrastructure.Configurations;

public class ScoreEventConfiguration :
    IEntityTypeConfiguration<ScoreEvent>
{
    /// <summary>
    /// Configures score event persistence.
    /// </summary>
    public void Configure(
        EntityTypeBuilder<ScoreEvent> builder)
    {
        builder.HasKey(x => x.Id);


        builder.Property(x => x.Description)
            .IsRequired()
            .HasMaxLength(250);


        builder.Property(x => x.Points)
            .IsRequired();


        builder.Property(x => x.Type)
            .HasConversion<int>();


        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey("UserId");


        builder.HasOne(x => x.League)
            .WithMany()
            .HasForeignKey("LeagueId");
    }
}