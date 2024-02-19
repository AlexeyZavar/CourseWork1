namespace Tonometer.Database.Entities;

public sealed class User
{
    public int Id { get; set; }

    public string UserName { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;

    public UserType Type { get; set; }
    public string? Email { get; set; } = null!;
    public string? Telegram { get; set; } = null!;

    public ICollection<Patient> Patients { get; set; } = new List<Patient>();
}

public enum UserType
{
    Patient,
    Watcher,
    Admin
}
