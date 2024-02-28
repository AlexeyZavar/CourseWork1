using Tonometer.Web.Data.Models.Dtos;

namespace Tonometer.Web.Data.Models;

public class SearchDevicesResponse
{
    public ICollection<MeasureDeviceDto> Data { get; set; }
}
