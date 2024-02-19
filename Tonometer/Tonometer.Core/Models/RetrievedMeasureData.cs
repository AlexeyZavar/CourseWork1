namespace Tonometer.Core.Models;

public sealed class RetrievedMeasureData
{
    public DateTime Time { get; set; }
    public decimal Sys { get; set; }
    public decimal Dia { get; set; }
    public decimal Pulse { get; set; }
}
