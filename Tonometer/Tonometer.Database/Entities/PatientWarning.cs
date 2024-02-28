namespace Tonometer.Database.Entities;

public sealed class PatientWarning
{
    public int Id { get; set; }

    public int PatientId { get; set; }
    public Patient Patient { get; set; } = null!;

    public int? MeasureDataId { get; set; }
    public MeasureData? MeasureData { get; set; }

    public string Message { get; set; } = null!;
    public DateTimeOffset Time { get; set; }
}
