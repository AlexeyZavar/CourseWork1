#region

using Tonometer.Web.Data.Models.Dtos;

#endregion

namespace Tonometer.Web.Data.Models;

public class SearchPatientsResponse
{
    public ICollection<PatientDto> Data { get; set; } = null!;
}
