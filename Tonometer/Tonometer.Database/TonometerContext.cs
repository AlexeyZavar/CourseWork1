#region

using Microsoft.EntityFrameworkCore;
using Tonometer.Database.Entities;

#endregion

namespace Tonometer.Database;

public sealed class TonometerContext : DbContext
{
    /// <inheritdoc />
    public TonometerContext(DbContextOptions options) : base(options)
    {
        Database.EnsureCreated();
    }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Patient> Patients { get; set; } = null!;
    public DbSet<MeasureDevice> MeasureDevices { get; set; } = null!;
    public DbSet<MeasureData> MeasureData { get; set; } = null!;
    public DbSet<PatientWarning> Warnings { get; set; } = null!;

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TonometerContext).Assembly);
    }
}
