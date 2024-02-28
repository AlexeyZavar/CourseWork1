#region

using Tonometer.Database.Entities;

#endregion

namespace Tonometer.Web.Data.Models;

public sealed class CreatePatientRequest
{
    public string UserName { get; set; } = null!;
    public string Password { get; set; } = null!;

    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;

    public Gender Gender { get; set; }
    public DateTime BirthDay { get; set; }
    public decimal Weight { get; set; }
}

public sealed class CreatePatientResponse
{
    public bool Success { get; set; }
    public string? Message { get; set; }
}
