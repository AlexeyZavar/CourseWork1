#region

using Tonometer.Web.Data.Models.Dtos;

#endregion

namespace Tonometer.Web.Data.Models;

public sealed class SearchDevicesResponse
{
    public ICollection<MeasureDeviceDto> Data { get; set; }
}
