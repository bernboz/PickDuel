using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PickDuel.Domain.Entities;

namespace PickDuel.Infrastructure.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    /// <summary>
    /// Configures user persistence.
    /// </summary>
    public void Configure(
        EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);


        builder.Property(x => x.Username)
            .IsRequired()
            .HasMaxLength(50);


        builder.Property(x => x.CreatedAt)
            .IsRequired();
    }
}