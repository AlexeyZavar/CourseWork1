namespace Tonometer.Database.Entities;

public sealed class MeasureDevice
{
    public int Id { get; set; }

    public string Manufacturer { get; set; } = null!;
    public string Model { get; set; } = null!;

    public ICollection<Patient> Patients { get; set; } = new List<Patient>();
    public ICollection<MeasureData> MeasureData { get; set; } = new List<MeasureData>();
}
