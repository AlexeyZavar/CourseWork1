namespace Tonometer.Web.Data.Models;

public class StatisticsResponse
{
    public long TotalPatients { get; set; }
    public long TotalMeasurements { get; set; }
    public long TotalDevices { get; set; }
    public long TotalWarnings { get; set; }

    public long TotalUsers { get; set; }
    public long TotalAdmins { get; set; }
    public long TotalDoctors { get; set; }
}
