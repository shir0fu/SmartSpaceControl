using Microsoft.AspNetCore.Mvc;
using SmartSpaceControl.Models.Models;
using SmartSpaceControl.Services;

namespace SmartSpaceControl.Controllers;

[ApiController]
public class UserController : ControllerBase
{
    private readonly ISessionService _sessionService;

    public UserController(ISessionService sessionService)
    {
        _sessionService = sessionService;
    }

    [HttpPost("/login")]
    public async Task<IActionResult> Login(UserCredential userCredential)
    {
        var token = await _sessionService.Authenticate(userCredential.Login, userCredential.Password);
        return Ok(token);
    }
}