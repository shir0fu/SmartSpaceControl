using Microsoft.AspNetCore.Mvc;
using SmartSpaceControl.Models.Models;
using SmartSpaceControl.Services;


namespace SmartSpaceControl.Controllers;

[ApiController]
public class UserController : ControllerBase
{
    private readonly ISessionService _sessionService;
    private readonly IUserService _userService;

    public UserController(ISessionService sessionService, IUserService userService)
    {
        _sessionService = sessionService;
        _userService = userService;
    }

    [HttpPost("/login")]
    public async Task<IActionResult> Login(UserCredential userCredential)
    {
        var token = await _sessionService.Authenticate(userCredential.Login, userCredential.Password);
        return Ok(token);
    }

    [HttpPost("/register")]
    public async Task<IActionResult> Register(UserRegisterModel userRegisterModel)
    {
        await _userService.RegisterUser(userRegisterModel);
        return Ok();
    }
}