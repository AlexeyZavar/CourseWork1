namespace Tonometer.Database.Entities;

public sealed class MeasureData
{
    public int Id { get; set; }

    public DateTime Time { get; set; }
    public decimal Sys { get; set; }
    public decimal Dia { get; set; }
    public decimal Pulse { get; set; }

    public int PatientId { get; set; }
    public Patient Patient { get; set; } = null!;

    public int MeasureDeviceId { get; set; }
    public MeasureDevice MeasureDevice { get; set; } = null!;
}
