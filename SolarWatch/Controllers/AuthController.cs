using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using SolarWatch.Service.Authentication;
using WeatherApi.Contracts;

namespace SolarWatch.Controllers;

[ApiController]
[Route("[controller]")]
[EnableCors("MyPolicy")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authenticationService)
    {
        _authService = authenticationService;
    }

    [HttpPost("Register")]
    public async Task<ActionResult> Register(RegistrationRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _authService.RegisterAsync(request.Email, request.Username, request.Password, request.Role);

        if (!result.Success)
        {
            AddErrors(result);
            return CreatedAtAction(nameof(Register), new RegistrationResponse(result.Email, result.UserName, result.Role, false));  
        }

        return CreatedAtAction(nameof(Register), new RegistrationResponse(result.Email, result.UserName, result.Role, true));
    }

    private void AddErrors(AuthResult result)
    {
        foreach (var error in result.ErrorMessages)
        {
            ModelState.AddModelError(error.Key, error.Value);
        }
    }
    
    [HttpPost("Login")]
    public async Task<ActionResult<AuthResponse>> Authenticate([FromBody] AuthRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _authService.LoginAsync(request.Email, request.Password);

        if (!result.Success)
        {
            AddErrors(result);
            return BadRequest(ModelState);
        }

        return Ok(new AuthResponse(result.Email, result.UserName, result.Token, result.Role));
    }
}