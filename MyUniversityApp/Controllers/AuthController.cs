using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniversityApp.Application.DTOs;
using UniversityApp.Application.Interfaces;
using UniversityApp.Domain.Entities;
using UniversityApp.Infrastructure.Data;

namespace MyUniversityApp.Controllers;

[ApiController]
[Route("api/[controller]")]

public class AuthController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly ITokenService _tokenService;

    public AuthController(
        ApplicationDbContext context,
        IPasswordHasher<User> passwordHasher,
        ITokenService tokenService)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        var userExists = await _context.Users.AnyAsync(user =>
            user.UserName == dto.UserName ||
            user.Email == dto.Email);

        if (userExists)
        {
            return BadRequest("Այս username-ը կամ email-ը արդեն օգտագործվում է։");
        }

        var user = new User
        {
            UserName = dto.UserName,
            Email = dto.Email,
            Role = "User"
        };

        user.PasswordHash =
            _passwordHasher.HashPassword(user, dto.Password);

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        return Ok(new
        {
            message = "Օգտատերը հաջողությամբ գրանցվեց։"
        });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(user =>
            user.UserName == dto.UserName);

        if (user is null)
        {
            return Unauthorized("Սխալ username կամ password։");
        }

        var passwordResult = _passwordHasher.VerifyHashedPassword(
            user,
            user.PasswordHash,
            dto.Password);

        if (passwordResult == PasswordVerificationResult.Failed)
        {
            return Unauthorized("Սխալ username կամ password։");
        }

        var tokens = _tokenService.CreateTokens(user);

        user.RefreshToken = tokens.RefreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

        await _context.SaveChangesAsync();

        return Ok(tokens);
    }
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(RefreshTokenRequestDto dto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(user =>
            user.RefreshToken == dto.RefreshToken);

        if (user is null)
        {
            return Unauthorized("Refresh token-ը սխալ է։");
        }

        if (user.RefreshTokenExpiryTime is null ||
            user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            return Unauthorized("Refresh token-ի ժամկետն ավարտվել է։");
        }

        var tokens = _tokenService.CreateTokens(user);

        user.RefreshToken = tokens.RefreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

        await _context.SaveChangesAsync();

        return Ok(tokens);
    }
    [HttpPost("logout")]
    public async Task<IActionResult> Logout(RefreshTokenRequestDto dto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(user =>
            user.RefreshToken == dto.RefreshToken);

        if (user is null)
        {
            return BadRequest("Refresh token-ը չի գտնվել։");
        }

        user.RefreshToken = null;
        user.RefreshTokenExpiryTime = null;

        await _context.SaveChangesAsync();

        return Ok(new
        {
            message = "Logout-ը հաջողությամբ կատարվեց։"
        });
    }
}