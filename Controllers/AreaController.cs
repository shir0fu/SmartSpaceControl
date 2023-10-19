﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartSpaceControl.Models.Dto;
using SmartSpaceControl.Models.Models;
using SmartSpaceControl.Services;
using System.Security.Claims;

namespace SmartSpaceControl.Controllers;

[ApiController]
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
        var user = await _userService.GetCurrentUser(HttpContext);
        var areas = await _areaService.GetAreas(user);
        return Ok(areas);
    }

    [HttpGet("/area/create")]
    public async Task<IActionResult> CreateArea(AreaDto newArea)
    {
        await _areaService.CreateArea(newArea);
        return Ok();
    }
}
