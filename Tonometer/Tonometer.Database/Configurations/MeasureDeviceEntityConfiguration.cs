#region

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tonometer.Database.Entities;

#endregion

namespace Tonometer.Database.Configurations;

public class MeasureDeviceEntityConfiguration : IEntityTypeConfiguration<MeasureDevice>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<MeasureDevice> builder)
    {
        builder
            .HasMany(x => x.Patients)
            .WithMany(x => x.Devices)
            .UsingEntity("PatientDevices");

        builder
            .HasMany(x => x.MeasureData)
            .WithOne(x => x.MeasureDevice);
    }
}
