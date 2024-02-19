namespace Tonometer.Web.Data.Models.Dtos;

public sealed class MeasureDataDto
{
    public DateTimeOffset Time { get; set; }
    public decimal Sys { get; set; }
    public decimal Dia { get; set; }
    public decimal Pulse { get; set; }
}
