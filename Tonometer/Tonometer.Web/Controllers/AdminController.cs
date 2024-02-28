#region

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tonometer.Database;
using Tonometer.Database.Entities;
using Tonometer.Web.Data.Models;
using Tonometer.Web.Filters;

#endregion

namespace Tonometer.Web.Controllers;

[ApiController]
[Authorize]
[EnsureAdmin]
[Route("admin")]
public class AdminController : ControllerBase
{
    private readonly TonometerContext _context;

    /// <inheritdoc />
    public AdminController(TonometerContext context)
    {
        _context = context;
    }

    [HttpGet("statistics")]
    public async Task<IActionResult> GetStatistics()
    {
        var patientsCount = await _context.Patients.CountAsync();
        var measuresCount = await _context.MeasureData.CountAsync();
        var devicesCount = await _context.MeasureDevices.CountAsync();
        var warningsCount = await _context.Warnings.CountAsync();

        var usersCount = await _context.Users.CountAsync();
        var doctorsCount = await _context.Users.CountAsync(x => x.Type == UserType.Watcher);
        var adminsCount = await _context.Users.CountAsync(x => x.Type == UserType.Admin);

        return Ok(new StatisticsResponse
        {
            TotalPatients = patientsCount,
            TotalMeasurements = measuresCount,
            TotalDevices = devicesCount,
            TotalWarnings = warningsCount,
            TotalUsers = usersCount,
            TotalDoctors = doctorsCount,
            TotalAdmins = adminsCount
        });
    }
}
