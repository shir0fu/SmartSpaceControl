using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartSpaceControl.Models.Dto;
using SmartSpaceControl.Models.Models;
using SmartSpaceControl.Services;


namespace SmartSpaceControl.Controllers;

[ApiController]
[Authorize]
public class AreaController : ControllerBase
{
    private readonly ISessionService _sessionService;
    private readonly IUserService _userService;
    private readonly IAreaService _areaService;

    public AreaController(ISessionService sessionService, IUserService userService, IAreaService areaService)
    {
        _sessionService = sessionService;
        _userService = userService;
        _areaService = areaService;
    }

    [HttpGet("/areas")]
    public async Task<IActionResult> GetAreas()
    {
        var areas = await _areaService.GetAreas();
        return Ok(areas);
    }

    [HttpGet("/areas")]
    public async Task<IActionResult> CreateArea(AreaDto newArea)
    {
        await _areaService.CreateArea(newArea);
        return Ok();
    }
}
