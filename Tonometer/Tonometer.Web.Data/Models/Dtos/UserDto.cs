#region

using Tonometer.Database.Entities;

#endregion

namespace Tonometer.Web.Data.Models.Dtos;

public sealed class UserDto
{
    public int Id { get; set; }
    public string UserName { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public UserType Type { get; set; }
    public string Email { get; set; } = null!;
    public ICollection<PatientDto> Patients { get; set; } = new List<PatientDto>();
}
