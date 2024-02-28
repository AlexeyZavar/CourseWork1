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
[EnsureAdmin]
[Route("device")]
public class DeviceController : ControllerBase
{
    private readonly TonometerContext _context;
    private readonly IMapper _mapper;

    /// <inheritdoc />
    public DeviceController(TonometerContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    [HttpGet("{deviceId:int}")]
    public async Task<IActionResult> GetDevice(int deviceId)
    {
        var device = await _context.MeasureDevices.FirstOrDefaultAsync(x => x.Id == deviceId);

        if (device == null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<MeasureDeviceDto>(device));
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchDevices([FromQuery] string? query)
    {
        var q = query?.Trim();
        var dbQuery = _context.MeasureDevices.AsQueryable();

        if (!string.IsNullOrWhiteSpace(q))
        {
            dbQuery = dbQuery.Where(x =>
                                        x.Id.ToString() == q ||
                                        x.Manufacturer.Contains(q) ||
                                        x.Model.Contains(q) ||
                                        x.Serial.Contains(q));
        }

        var devices = await dbQuery
                            .OrderBy(x => x.Id)
                            .Take(100)
                            .ToListAsync();

        return Ok(new SearchDevicesResponse
        {
            Data = _mapper.Map<List<MeasureDeviceDto>>(devices)
        });
    }
    
    [HttpPost("create")]
    public async Task<IActionResult> CreateDevice([FromBody] CreateDeviceRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Manufacturer) ||
            string.IsNullOrWhiteSpace(request.Model) ||
            string.IsNullOrWhiteSpace(request.Serial))
        {
            return BadRequest(new CreateDeviceResponse
            {
                Success = false,
                Message = "Все поля должны быть заполнены."
            });
        }
        
        var device = new MeasureDevice
        {
            Manufacturer = request.Manufacturer,
            Model = request.Model,
            Serial = request.Serial
        };

        await _context.MeasureDevices.AddAsync(device);
        await _context.SaveChangesAsync();

        return Ok(new CreateDeviceResponse
        {
            Success = true
        });
    }
}
