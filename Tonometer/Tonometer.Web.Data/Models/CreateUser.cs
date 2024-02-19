#region

using Tonometer.Database.Entities;

#endregion

namespace Tonometer.Web.Data.Models;

public class CreateUserRequest
{
    public string UserName { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Password { get; set; } = null!;

    public UserType Type { get; set; }
}

public class CreateUserResponse
{
    public bool Success { get; set; }
    public string? Message { get; set; }
}
