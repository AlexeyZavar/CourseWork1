#region

using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tonometer.Database;
using Tonometer.Database.Entities;
using Tonometer.Web.Data.Models;
using Tonometer.Web.Data.Models.Dtos;
using Tonometer.Web.Filters;

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

    [HttpGet("{patientId:int}")]
    [EnsureAccessToPatient]
    public async Task<IActionResult> GetPatientData(int patientId)
    {
        var patient = await _context.Patients
                                    .Include(x => x.Devices)
                                    .FirstOrDefaultAsync(x => x.Id == patientId);

        if (patient == null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<PatientDto>(patient));
    }

    [HttpGet("{patientId:int}/data")]
    [EnsureAccessToPatient]
    public async Task<IActionResult> GetPatientData(int patientId, [FromQuery] DateTime from, [FromQuery] DateTime to)
    {
        if (from > DateTime.UtcNow || from > to)
        {
            return BadRequest();
        }

        var data = await _context.MeasureData
                                 .Where(x => x.PatientId == patientId && x.Time >= from && x.Time <= to)
                                 .OrderByDescending(x => x.Id)
                                 .Take(1000)
                                 .ToListAsync();

        return Ok(new PatientMeasurements
        {
            Data = _mapper.Map<List<MeasureDataDto>>(data)
        });
    }

    [HttpGet("{patientId:int}/warnings")]
    [EnsureAccessToPatient]
    public async Task<IActionResult> GetPatientWarnings(int patientId)
    {
        var data = await _context.Warnings
                                 .Where(x => x.PatientId == patientId)
                                 .OrderByDescending(x => x.Id)
                                 .Take(100)
                                 .ToListAsync();

        return Ok(new PatientWarnings
        {
            Data = _mapper.Map<List<PatientWarningDto>>(data)
        });
    }

    [HttpPost("create")]
    [EnsureAdmin]
    public async Task<IActionResult> CreatePatient(CreatePatientRequest req)
    {
        if (string.IsNullOrWhiteSpace(req.UserName))
        {
            return BadRequest(new CreatePatientResponse
            {
                Success = false,
                Message = "Проверьте юзернейм."
            });
        }

        if (string.IsNullOrWhiteSpace(req.Password))
        {
            return BadRequest(new CreatePatientResponse
            {
                Success = false,
                Message = "Проверьте пароль."
            });
        }

        if (string.IsNullOrWhiteSpace(req.FirstName) || string.IsNullOrWhiteSpace(req.LastName))
        {
            return BadRequest(new CreatePatientResponse
            {
                Success = false,
                Message = "Проверьте имя человека."
            });
        }

        if (req.BirthDay.Year < 1900 || req.BirthDay.Year > DateTime.UtcNow.Year)
        {
            return BadRequest(new CreatePatientResponse
            {
                Success = false,
                Message = "Проверьте дату рождения."
            });
        }

        if (req.Weight is < 40 or > 300)
        {
            return BadRequest(new CreatePatientResponse
            {
                Success = false,
                Message = "Проверьте вес."
            });
        }
        
        // todo: case insensitive username comparison
        var userExists = await _context.Users.AnyAsync(x => x.UserName == req.UserName);
        if (userExists)
        {
            return BadRequest(new CreatePatientResponse
            {
                Success = false,
                Message = "Пользователь с таким юзернеймом уже существует."
            });
        }

        var user = new User
        {
            UserName = req.UserName,
            PasswordHash = CryptoUtilities.HashPassword(req.Password),
            FirstName = req.FirstName,
            LastName = req.LastName,
            Type = UserType.Patient
        };

        var patient = new Patient
        {
            FirstName = req.FirstName,
            LastName = req.LastName,
            Gender = req.Gender,
            BirthDay = req.BirthDay,
            Weight = req.Weight,
            Watchers =
            {
                user
            }
        };

        _context.Add(user);
        _context.Add(patient);

        await _context.SaveChangesAsync();

        return Ok(new CreatePatientResponse
        {
            Success = true
        });
    }
}
