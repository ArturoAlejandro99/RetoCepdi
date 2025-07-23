using Microsoft.AspNetCore.Mvc;
using RetoTecnico.API.DTOs;
using RetoTecnico.API.Services.Interfaces;
using System.Threading.Tasks;

namespace RetoTecnico.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var token = await _authService.LoginAsync(loginDto);

        if (token == null)
        {
            return Unauthorized(new { message = "Credenciales inválidas." });
        }

        return Ok(new { Token = token });
    }
}