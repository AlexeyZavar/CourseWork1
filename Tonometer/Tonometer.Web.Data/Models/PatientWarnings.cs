#region

using Tonometer.Web.Data.Models.Dtos;

#endregion

namespace Tonometer.Web.Data.Models;

public sealed class PatientWarnings
{
    public ICollection<PatientWarningDto> Data { get; set; } = null!;
}
