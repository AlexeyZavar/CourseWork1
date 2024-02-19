#region

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tonometer.Database.Entities;

#endregion

namespace Tonometer.Database.Configurations;

public class PatientEntityConfiguration : IEntityTypeConfiguration<Patient>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Patient> builder)
    {
        builder
            .HasMany(x => x.Watchers)
            .WithMany(x => x.Patients)
            .UsingEntity("PatientAccesses");

        builder
            .HasMany(x => x.MeasureData)
            .WithOne(x => x.Patient);

        builder
            .HasMany(x => x.Devices)
            .WithMany(x => x.Patients)
            .UsingEntity("PatientDevices");
    }
}
