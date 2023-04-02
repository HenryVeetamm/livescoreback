using System.Diagnostics;
using System.Net;
using Domain.Identity;
using Interfaces.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PublicAPI.DTO.Session;

namespace API.Controllers;

[Route("session")]
[ApiController]
public class SessionController : ControllerBase
{
    private readonly ISessionService _sessionService;

    public SessionController(ISessionService sessionService)
    {
        _sessionService = sessionService;
    }
    
    /// <summary>
    /// Register a new user
    /// </summary>
    /// <param name="registrationDto">Registration information</param>
    /// <returns>JWT response with JWT token</returns>
    [HttpPost("register")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AuthorizationDto>> Register([FromBody] RegisterDto registrationDto)
    {
        var result = await _sessionService.Register(registrationDto);

        return Ok(result);
    }
    
    /// <summary>
    /// Register a new user
    /// </summary>
    /// <param name="registrationDto">Registration information</param>
    /// <returns>JWT response with JWT token</returns>
    [HttpPost("login")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AuthorizationDto>> Login([FromBody] LoginDto loginDto)
    {
        var result = await _sessionService.Login(loginDto);

        return Ok(result);
    }
    
  
}