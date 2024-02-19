#region

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tonometer.Database.Entities;

#endregion

namespace Tonometer.Database.Configurations;

public class MeasureDataEntityConfiguration : IEntityTypeConfiguration<MeasureData>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<MeasureData> builder)
    {
        builder
            .HasOne(x => x.Patient)
            .WithMany(x => x.MeasureData);
    }
}
