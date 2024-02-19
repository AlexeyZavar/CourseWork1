#region

using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tonometer.Database;
using Tonometer.Web.Data.Models;
using Tonometer.Web.Data.Models.Dtos;
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
}
