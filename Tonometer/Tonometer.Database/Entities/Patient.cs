namespace Tonometer.Database.Entities;

public sealed class Patient
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;

    public Gender Gender { get; set; }
    public DateTime BirthDay { get; set; }
    public decimal Weight { get; set; }

    public ICollection<User> Watchers { get; set; } = new List<User>();
    public ICollection<MeasureData> MeasureData { get; set; } = new List<MeasureData>();
    public ICollection<MeasureDevice> Devices { get; set; } = new List<MeasureDevice>();
}

public enum Gender
{
    Male,
    Female
}
