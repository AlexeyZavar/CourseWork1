#region

using Tonometer.Web.Data.Models.Dtos;

#endregion

namespace Tonometer.Web.Data.Models;

public sealed class PatientMeasurements
{
    public ICollection<MeasureDataDto> Data { get; set; } = null!;
}
