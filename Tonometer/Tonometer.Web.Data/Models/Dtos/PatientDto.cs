#region

using Tonometer.Database.Entities;

#endregion

namespace Tonometer.Web.Data.Models.Dtos;

public sealed class PatientDto
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;

    public Gender Gender { get; set; }
    public int Age { get; set; }
    public decimal Weight { get; set; }

    public ICollection<MeasureDeviceDto> Devices { get; set; } = new List<MeasureDeviceDto>();
}
