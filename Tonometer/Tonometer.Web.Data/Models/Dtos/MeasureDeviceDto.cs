namespace Tonometer.Web.Data.Models.Dtos;

public sealed class MeasureDeviceDto
{
    public int Id { get; set; }

    public string Manufacturer { get; set; } = null!;
    public string Model { get; set; } = null!;
    public string Serial { get; set; } = null!;
}
