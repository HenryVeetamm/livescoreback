using Domain;
using Interfaces.Converters;
using Interfaces.Hubs;
using Interfaces.Services;
using Microsoft.AspNetCore.SignalR;
using PublicAPI.DTO.PlayerInGame;

namespace Services.Hubs;

public class LiveGameHubContext : ILiveGameHubContext
{
    private readonly IHubContext<LiveGameHub, ILiveGameClient> _hubContext;
    private readonly IPlayerInGameConverter _playerInGameConverter;
    private readonly ISetConverter _setConverter;
    private readonly IGameConverter _gameConverter;
    private readonly IHttpContextService _httpContextService;

    public LiveGameHubContext(
        IHubContext<LiveGameHub, ILiveGameClient> hubContext,
        IPlayerInGameConverter playerInGameConverter,
        ISetConverter setConverter,
        IGameConverter gameConverter,
        IHttpContextService httpContextService)
    {
        _hubContext = hubContext;
        _playerInGameConverter = playerInGameConverter;
        _setConverter = setConverter;
        _gameConverter = gameConverter;
        _httpContextService = httpContextService;
    }
    public async Task AddPointToGameAsync(Guid gameId)
    {
        await _hubContext.Clients.GroupExcept(gameId.ToString(), GetMyConnection()).AddPointToGame();
    }

    public async Task PlayerDataChangedAsync(Guid teamId, Guid gameId, PlayerInGame playerInGame, ManagePlayerPointsDto playerDataChangesDto)
    {
        var dto = _playerInGameConverter.Convert(playerInGame);
        
        await _hubContext.Clients.Group(gameId.ToString()).PlayerDataChanged(teamId, gameId, dto, playerDataChangesDto);
    }

    public async Task GameScoreChangedAsync(Guid gameId, Set set)
    {
        var dto = _setConverter.Convert(set);
        await _hubContext.Clients.GroupExcept(gameId.ToString(), GetMyConnection()).GameScoreChanged(gameId, dto);
        
    }

    public async Task GameStartedAsync(Game game, Set set)
    {
        var dto = _gameConverter.Convert(game);
        var setDto = _setConverter.Convert(set);
        await _hubContext.Clients.GroupExcept(game.Id.ToString(), GetMyConnection()).GameStarted(dto, setDto);
    }

    public async Task StartNewSetAsync(Guid gameId, Set set)
    {
        var dto = _setConverter.Convert(set);
        await _hubContext.Clients.GroupExcept(gameId.ToString(), GetMyConnection()).StartNewSet(gameId, dto);
    }

    public async Task EndGameAsync(Guid gameId, Game game)
    {
        var dto = _gameConverter.Convert(game);
        await _hubContext.Clients.GroupExcept(gameId.ToString(), GetMyConnection()).EndGame(dto);
        
    }

    public async Task PlayersChangedAsync(Guid gameId, Guid teamId, PlayerInGame[] playerInGameDtos)
    {
        var dtos = _playerInGameConverter.ConvertAll(playerInGameDtos);
        await _hubContext.Clients.GroupExcept(gameId.ToString(), GetMyConnection()).PlayersChanged(teamId, dtos);
    }

    private List<string> GetMyConnection()
    {
        var userId = _httpContextService.GetUserId();
        var connections = new List<string>();
        Console.BackgroundColor = ConsoleColor.Magenta;
        
        Console.WriteLine(userId);
        if (userId == Guid.Empty) return connections;
        var myConnection = LiveGameHub.UserConnections.FirstOrDefault(kv => kv.Value == userId).Key;
        
        if (myConnection != null) connections.Add(myConnection);
        foreach (var connection in connections)
        {
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.Write("Id");
            Console.WriteLine(connection);
        }
        return connections;
    }
}