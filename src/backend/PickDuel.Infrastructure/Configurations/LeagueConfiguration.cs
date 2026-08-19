using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PickDuel.Domain.Entities;

namespace PickDuel.Infrastructure.Configurations;

public class LeagueConfiguration : IEntityTypeConfiguration<League>
{
    /// <summary>
    /// Configures league persistence.
    /// </summary>
    public void Configure(
        EntityTypeBuilder<League> builder)
    {
        builder.HasKey(x => x.Id);


        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100);
    }
}