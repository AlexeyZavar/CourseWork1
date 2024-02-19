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
using Tonometer.Web.Services;

#endregion

namespace Tonometer.Web.Controllers;

[ApiController]
[Authorize]
[Route("user")]
public class UserController : ControllerBase
{
    private readonly TonometerContext _context;
    private readonly TokenService _tokenService;
    private readonly IMapper _mapper;

    /// <inheritdoc />
    public UserController(TonometerContext context, TokenService tokenService, IMapper mapper)
    {
        _context = context;
        _tokenService = tokenService;
        _mapper = mapper;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequest req)
    {
        // todo: case insensitive username comparison
        var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == req.Username &&
                                                                 x.PasswordHash ==
                                                                 CryptoUtilities.HashPassword(req.Password));

        if (user == null)
        {
            return BadRequest(new LoginResponse
            {
                Success = false,
                Message = "Пользователь не найден."
            });
        }

        var token = _tokenService.GenerateToken(user);

        return Ok(new LoginResponse
        {
            Success = true,
            Token = token
        });
    }

    [HttpPost("logout")]
    public Task<IActionResult> Logout()
    {
        return Task.FromResult<IActionResult>(Ok());
    }


    [HttpGet("session")]
    public async Task<IActionResult> Session()
    {
        var userId = this.GetUserId();
        var user = await _context.Users
                                 .Include(x => x.Patients)
                                 .FirstOrDefaultAsync(x => x.Id == userId);

        if (user == null)
        {
            return BadRequest();
        }

        return Ok(_mapper.Map<UserDto>(user));
    }

    [HttpGet("search")]
    [EnsureAdmin]
    public async Task<IActionResult> Search([FromQuery] string? query)
    {
        var q = query?.Trim();
        var dbQuery = _context.Users.AsQueryable();

        if (!string.IsNullOrWhiteSpace(q))
        {
            dbQuery = dbQuery.Where(x => x.UserName.Contains(q) ||
                                         x.FirstName.Contains(q) ||
                                         x.LastName.Contains(q));
        }

        var users = await dbQuery
                          .OrderBy(x => x.Id)
                          .Take(100)
                          .ToListAsync();

        return Ok(new SearchUsersResponse
        {
            Data = _mapper.Map<List<UserDto>>(users)
        });
    }

    [HttpGet("{userId:int}")]
    [EnsureAdmin]
    public async Task<IActionResult> GetUser(int userId)
    {
        var user = await _context.Users
                                 .Include(x => x.Patients)
                                 .FirstOrDefaultAsync(x => x.Id == userId);

        if (user == null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<UserDto>(user));
    }

    [HttpPost("create")]
    [EnsureAdmin]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest req)
    {
        if (string.IsNullOrWhiteSpace(req.UserName))
        {
            return BadRequest(new CreateUserResponse
            {
                Success = false,
                Message = "Имя пользователя не может быть пустым."
            });
        }

        if (string.IsNullOrWhiteSpace(req.Password))
        {
            return BadRequest(new CreateUserResponse
            {
                Success = false,
                Message = "Пароль не может быть пустым."
            });
        }

        if (string.IsNullOrWhiteSpace(req.FirstName))
        {
            return BadRequest(new CreateUserResponse
            {
                Success = false,
                Message = "Имя не может быть пустым."
            });
        }

        if (string.IsNullOrWhiteSpace(req.LastName))
        {
            return BadRequest(new CreateUserResponse
            {
                Success = false,
                Message = "Фамилия не может быть пустой."
            });
        }

        if (req.Type == UserType.Patient)
        {
            return BadRequest(new CreateUserResponse
            {
                Success = false,
                Message = "Нельзя создать пациента через этот метод."
            });
        }

        if (await _context.Users.AnyAsync(x => x.UserName == req.UserName))
        {
            return BadRequest(new CreateUserResponse
            {
                Success = false,
                Message = "Пользователь с таким именем уже существует."
            });
        }

        var user = new User
        {
            UserName = req.UserName,
            PasswordHash = CryptoUtilities.HashPassword(req.Password),
            FirstName = req.FirstName,
            LastName = req.LastName,
            Type = req.Type
        };

        _context.Add(user);
        await _context.SaveChangesAsync();

        return Ok(new CreateUserResponse
        {
            Success = true
        });
    }
}
