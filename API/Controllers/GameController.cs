using Interfaces.Converters;
using Interfaces.Repositories;
using Interfaces.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PublicAPI.DTO.Game;

namespace API.Controllers;

[Route("game")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class GameController : Controller
{
    private readonly IGameService _gameService;
    private readonly IGameRepository _gameRepository;
    private readonly IGameConverter _gameConverter;
    private readonly ISetConverter _setConverter;
    private readonly ISetRepository _setRepository;

    public GameController(
        IGameService gameService, 
        IGameRepository gameRepository, 
        IGameConverter gameConverter,
        ISetConverter setConverter,
        ISetRepository setRepository)
    {
        _gameService = gameService;
        _gameRepository = gameRepository;
        _gameConverter = gameConverter;
        _setConverter = setConverter;
        _setRepository = setRepository;
    }
    
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult GetGames([FromQuery] int page, [FromQuery] int pageSize)
    {
        
        var result = _gameRepository.GetGames(page, pageSize);
        var dto = _gameConverter.ConvertAll(result);
        
        return Ok(dto);
    }
    
    [HttpPost]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult CreateGame([FromBody] AddGameDto gameDto)
    {
        var game = _gameService.AddGame(gameDto);
        var dto = _gameConverter.Convert(game);

        return Ok(dto);
    }

    [HttpGet("{gameId}")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [AllowAnonymous]
    public IActionResult GetGameById(Guid gameId)
    {
        var game = _gameRepository.GetGameById(gameId);
        var dto = _gameConverter.Convert(game);
        
        return Ok(dto);
    }
    
    [HttpPost("{gameId}/player")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult AddPlayerToGame(AddPlayerToGameDto dto)
    {
        _gameService.AddPlayerToGame(dto);
        return Ok();
    }
    
    //
    [HttpPost("{gameId}/add-score")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult ManageGameScore(ManageGameScoreDto dto)
    {
        var set = _gameService.ManageGameScore(dto);
        var result = _setConverter.Convert(set);
        return Ok(result);
    }
    
    [HttpPost("{gameId}/start-game")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult StartGame(Guid gameId)
    {
        var set = _gameService.StartGame(gameId);
        var result = _setConverter.Convert(set);
        return Ok(result);
    }
    
    [HttpPost("{gameId}/startnewset")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult StartNewSet(Guid gameId)
    {
        var set = _gameService.StartNewSet(gameId);
        var result = _setConverter.Convert(set);
        return Ok(result);
    }
    
    [HttpGet("{gameId}/currentset")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [AllowAnonymous]
    public IActionResult GetCurrentSet(Guid gameId)
    {
        var set = _gameService.GetCurrentSet(gameId);
        var result = _setConverter.Convert(set);
        return Ok(result);
    }

    [HttpGet("my-games")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult GetMyGames()
    {
        var result = _gameService.GetMyGames();
        var dto = _gameConverter.ConvertAll(result);
        
        return Ok(dto);
    }
    
    [HttpGet("{gameId}/completedsets")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [AllowAnonymous]
    public IActionResult GetTeamById(Guid gameId)
    {
        var sets = _setRepository.GetCompletedSetsByGameId(gameId);
        var result = _setConverter.ConvertAll(sets);
        return Ok(result);
    }
    
    [HttpPost("{gameId}/endgame")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult EndGame(Guid gameId)
    {
        var game = _gameService.EndGame(gameId);
        var result = _gameConverter.Convert(game);
        return Ok(result);
    }
    



}