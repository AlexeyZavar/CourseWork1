#region

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tonometer.Database.Entities;

#endregion

namespace Tonometer.Database.Configurations;

public class UserEntityConfiguration : IEntityTypeConfiguration<User>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder
            .HasMany(x => x.Patients)
            .WithMany(x => x.Watchers);
    }
}
