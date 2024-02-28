namespace Tonometer.Web.Data.Models;

public sealed class CreateDeviceRequest
{
    public string Manufacturer { get; set; } = null!;
    public string Model { get; set; } = null!;
    public string Serial { get; set; } = null!;
}

public sealed class CreateDeviceResponse
{
    public bool Success { get; set; }
    public string? Message { get; set; }
}
