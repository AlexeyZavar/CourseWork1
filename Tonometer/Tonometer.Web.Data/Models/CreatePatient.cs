using Tonometer.Database.Entities;

namespace Tonometer.Web.Data.Models;

public class CreatePatientRequest
{
    public string UserName { get; set; } = null!;
    public string Password { get; set; } = null!;

    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;

    public Gender Gender { get; set; }
    public DateTime BirthDay { get; set; }
    public decimal Weight { get; set; }
}

public class CreatePatientResponse
{
    public bool Success { get; set; }
    public string? Message { get; set; }
}