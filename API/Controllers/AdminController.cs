using Interfaces.Converters;
using Interfaces.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PublicAPI.DTO.Session;

namespace API.Controllers;

[Route("admin")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class AdminController : Controller
{
    private readonly IAdminService _adminService;

    private readonly IUserConverter _userConverter;
    //Get All users
    //Register users
    //InActivate user

    public AdminController(IAdminService adminService, IUserConverter userConverter)
    {
        _adminService = adminService;
        _userConverter = userConverter;
    }
    
    [HttpGet("users")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult GetUsers()
    {
        var result = _adminService.GetSystemUsers();
        var dto = _userConverter.ConvertAll(result);
        
        return Ok(dto);
    }

    
    /// <summary>
    /// Register a new user
    /// </summary>
    /// <param name="registrationDto">Registration information</param>
    /// <returns>JWT response with JWT token</returns>
    [HttpPost("updatepassword")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordDto dto)
    {
         await _adminService.UpdatePassword(dto);

        return Ok();
    }
    
    /// <summary>
    /// Register a new user
    /// </summary>
    /// <param name="registrationDto">Registration information</param>
    /// <returns>JWT response with JWT token</returns>
    [HttpPost("adduser")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddUser([FromBody] RegisterDto dto)
    {
        await _adminService.RegisterUser(dto);

        return Ok();
    }
}