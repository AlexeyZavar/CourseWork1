namespace Tonometer.Web.Data.Models.Dtos;

public class PatientWarningDto
{
    public int Id { get; set; }

    public int PatientId { get; set; }

    public int? MeasureDataId { get; set; }

    public string Message { get; set; } = null!;
    public DateTimeOffset Time { get; set; }
}
