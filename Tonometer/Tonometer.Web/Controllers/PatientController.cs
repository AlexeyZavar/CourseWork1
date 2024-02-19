#region

using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tonometer.Database;
using Tonometer.Web.Data.Models;
using Tonometer.Web.Data.Models.Dtos;

#endregion

namespace Tonometer.Web.Controllers;

[ApiController]
[Authorize]
[Route("patient")]
public class PatientController : ControllerBase
{
    private readonly TonometerContext _context;
    private readonly IMapper _mapper;

    /// <inheritdoc />
    public PatientController(TonometerContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetPatientData(int id)
    {
        // todo: check if really have access to patient

        var patient = await _context.Patients
                                    .Include(x => x.Devices)
                                    .FirstOrDefaultAsync(x => x.Id == id);

        if (patient == null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<PatientDto>(patient));
    }

    [HttpGet("{id:int}/data")]
    public async Task<IActionResult> GetPatientData(int id, [FromQuery] DateTime from, [FromQuery] DateTime to)
    {
        if (from > DateTime.UtcNow || from > to)
        {
            return BadRequest();
        }

        var data = await _context.MeasureData
                                 .Where(x => x.PatientId == id && x.Time >= from && x.Time <= to)
                                 .OrderByDescending(x => x.Id)
                                 .Take(1000)
                                 .ToListAsync();

        return Ok(new PatientMeasurements
        {
            Data = _mapper.Map<List<MeasureDataDto>>(data)
        });
    }

    [HttpGet("{id:int}/warnings")]
    public async Task<IActionResult> GetPatientWarnings(int id)
    {
        var data = await _context.Warnings
                                 .Where(x => x.PatientId == id)
                                 .OrderByDescending(x => x.Id)
                                 .Take(100)
                                 .ToListAsync();

        return Ok(new PatientWarnings
        {
            Data = _mapper.Map<List<PatientWarningDto>>(data)
        });
    }
}
