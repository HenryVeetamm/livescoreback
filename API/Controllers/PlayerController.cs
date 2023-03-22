using Interfaces.Converters;
using Interfaces.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PublicAPI.DTO.Player;
using PublicAPI.DTO.PlayerInGame;

namespace API.Controllers;

[Route("player")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class PlayerController : Controller
{
    private readonly IPlayerService _playerService;
    private readonly IPlayerConverter _playerConverter;
    private readonly IAddPlayerToGameConverter _addPlayerToGameConverter;
    private readonly IPlayerInGameConverter _playerInGameConverter;
    private readonly IPlayerInGameService _playerInGameService;

    //Planned endpoints
    //1.Add player to team
    //2.Edit player in team
    //3.

    public PlayerController(
        IPlayerService playerService, 
        IPlayerConverter playerConverter, 
        IAddPlayerToGameConverter addPlayerToGameConverter,
        IPlayerInGameConverter playerInGameConverter,
        IPlayerInGameService playerInGameService)
    {
        _playerService = playerService;
        _playerConverter = playerConverter;
        _addPlayerToGameConverter = addPlayerToGameConverter;
        _playerInGameConverter = playerInGameConverter;
        _playerInGameService = playerInGameService;
    }
    
    [HttpPost]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult AddPlayerToTeam([FromBody] AddPlayerDto playerDto)
    {
        var player = _playerService.AddPlayerToTeam(playerDto);
        var dto = _playerConverter.Convert(player);
        
        return Ok(dto);
    }
    
    [HttpPut]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult EditPlayer([FromBody] UpdatePlayerDto playerDto)
    {
        var player = _playerService.EditPlayer(playerDto);
        var dto = _playerConverter.Convert(player);
        
        return Ok(dto);
    }
    
    [HttpGet("team/{teamId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [AllowAnonymous]
    public IActionResult GetTeamPlayers(Guid teamId)
    {
        var result = _playerService.GetTeamPlayers(teamId);
        var dto = _playerConverter.ConvertAll(result);
        
        return Ok(dto);
    }
    
    [HttpGet("my-team")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult GetMyTeamPlayers()
    {
        var result = _playerService.GetMyTeamPlayers();
        
        var dto = _playerConverter.ConvertAll(result);
        
        return Ok(dto);
    }

    [HttpPost("{playerId}/picture")]
    [Consumes("multipart/form-data")]
    public IActionResult UploadPicture([FromForm] IFormFile file, Guid playerId)
    {
        _playerService.UploadProfilePicture(file, playerId);
        return Ok();
    }
    
    [HttpGet("my-team/game/{gameId}")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult GetForAddingToGame(Guid gameId)
    {
        var gamePlayer = _playerService.GetForAddingToGame(gameId);
        var dtos = _addPlayerToGameConverter.ConvertAll(gamePlayer, gameId);
       
        return Ok(dtos);
    }
    
    [HttpGet("game/{gameId}/team/{teamId}")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [AllowAnonymous]
    public IActionResult GetForGame(Guid gameId, Guid teamId)
    {
        var gamePlayer = _playerService.GetByGameId(gameId, teamId);
        var res = _playerInGameConverter.ConvertAll(gamePlayer);
        
        return Ok(new { Result = res, TeamId = teamId} );
    }
    
    [HttpPost("{playerId}/team/{teamId}/game/{gameId}")]
    [Produces("application/json")]
    [Consumes("application/json")]
    public IActionResult ManagePlayerInGameResult(ManagePlayerPointsDto playerPointsDto, Guid teamId, Guid gameId)
    {
        var result = _playerInGameService.ManagePlayerResult(playerPointsDto, teamId, gameId);
        var res = _playerInGameConverter.Convert(result);
        return Ok(new { TeamId = teamId, Result = res});
    }
    
    [HttpGet("{playerId}/statistics")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [AllowAnonymous]
    public IActionResult GetPlayerStatistics(Guid playerId)
    {
        var playerStatistics = _playerService.GetPlayerStatistics(playerId);

        return Ok(playerStatistics);
    }

}