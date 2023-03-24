using Interfaces.Converters;
using Interfaces.Repositories;
using Interfaces.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PublicAPI.DTO.Team;

namespace API.Controllers;

[Route("team")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class TeamController : Controller
{
    private readonly ITeamService _teamService;
    private readonly ITeamConverter _teamConverter;
    private readonly ITeamRepository _teamRepository;

    public TeamController(ITeamService teamService, ITeamConverter teamConverter, ITeamRepository teamRepository)
    {
        _teamService = teamService;
        _teamConverter = teamConverter;
        _teamRepository = teamRepository;
    }
    
    [HttpPost]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult AddTeam([FromBody] CreateTeamDto createTeamDto)
    {
        var result = _teamService.AddTeam(createTeamDto);
        var dto = _teamConverter.Convert(result);
        return Ok(dto);
    }
    
    [HttpPut]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult UpdateTeam([FromBody] UpdateTeamDto updateTeamDto)
    {
        var result = _teamService.UpdateTeam(updateTeamDto);
        var dto = _teamConverter.Convert(result);
        return Ok(dto);    }
    
    [HttpGet("my-team")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult GetMyTeam()
    {
        var result = _teamService.GetMyTeam();
        var dto = _teamConverter.Convert(result);
        return Ok(dto);
    }
    
    [HttpGet]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult GetAllTeamsForSearch()
    {
        var result = _teamService.GetTeams();
        var dto = _teamConverter.ConvertAll(result);
        return Ok(dto);
    }
    
    [HttpPost("{teamId}/logo")]
    [Consumes("multipart/form-data")]
    public IActionResult UploadPicture([FromForm] IFormFile file, Guid teamId)
    {
        _teamService.UploadTeamLogo(file, teamId);
        return Ok();
    }
    
    
    [HttpGet("all")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [AllowAnonymous]
    public IActionResult GetAllTeams()
    {
        var result = _teamRepository.GetTeamsWithPictures();
        var dto = _teamConverter.ConvertAll(result);
        return Ok(dto);
    }
    
    [HttpGet("userteamId")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [AllowAnonymous]
    public IActionResult GetUserTeamId()
    {
        var result = _teamService.GetMyTeam();
        return Ok(result.Id);
    }
    
    [HttpGet("{teamId}")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [AllowAnonymous]
    public IActionResult GetTeamById(Guid teamId)
    {
        var result = _teamRepository.GetTeamById(teamId);
        var dto = _teamConverter.Convert(result);
        return Ok(dto);
    }
    
    
    
    
}