using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PickDuel.Domain.Entities;

namespace PickDuel.Infrastructure.Configurations;

public class GameConfiguration : IEntityTypeConfiguration<Game>
{
    /// <summary>
    /// Configures game persistence.
    /// </summary>
    public void Configure(
        EntityTypeBuilder<Game> builder)
    {
        builder.HasKey(x => x.Id);


        builder.Property(x => x.HomeTeam)
            .IsRequired()
            .HasMaxLength(100);


        builder.Property(x => x.AwayTeam)
            .IsRequired()
            .HasMaxLength(100);


        builder.Property(x => x.StartTime)
            .IsRequired();
    }
}