using Exceptions;
using Interfaces.Hubs;
using Interfaces.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.AspNetCore.SignalR;
using PublicAPI.DTO.PlayerInGame;

namespace Services.Hubs;
[AllowAnonymous]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class LiveGameHub : Hub<ILiveGameClient>, ILiveGameHub
{
    private readonly IHttpContextService _httpContextService;
    private readonly IGameService _gameService;
    private static Dictionary<Guid, int> ConnectionCounts = new Dictionary<Guid, int>();
    public static Dictionary<string, Guid> UserConnections = new Dictionary<string, Guid>();


    public LiveGameHub(IHttpContextService httpContextService, IGameService gameService)
    {
        _httpContextService = httpContextService;
        _gameService = gameService;
    }
    
  
    public override async Task OnConnectedAsync()
    {
        var userId = _httpContextService.GetUserId();
        var gameId = GetGameId();
        var connectionId = Context.ConnectionId;
        
        Console.BackgroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"HUB CONNECTION Got called {Context.ConnectionId} UserId: {userId} GameID:{gameId}");
        
        await Groups.AddToGroupAsync(connectionId, gameId.ToString());

        AddUserToGame(connectionId, userId);
        
        var viewers = AddViewerToGame(gameId);
        _gameService.CheckForMaxViews(gameId, viewers);
        
        await Clients.OthersInGroup(gameId.ToString()).PersonJoined(viewers);
        await Clients.Caller.Connected(viewers);
            
        await base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        Console.BackgroundColor = ConsoleColor.Red;
        Console.WriteLine("Disconnected");
        var gameId = GetGameId();
        var connectionId = Context.ConnectionId;
        var userId = _httpContextService.GetUserId();
        
        var viewers = RemoveViewerFromGame(gameId);
        
        RemoveUserFromGame(connectionId, userId);

        Clients.OthersInGroup(gameId.ToString()).PersonLeft(viewers);

        return base.OnDisconnectedAsync(exception);
    }

    private Guid GetGameId()
    {
        var gameId = Context.GetHttpContext().Request.Query["gameId"];
        if (Guid.TryParse(gameId, out Guid result)) return result;
        
        throw new LogicException("GameID not found");
    }

    private int AddViewerToGame(Guid gameId)
    {
        if (ConnectionCounts.ContainsKey(gameId)) ConnectionCounts[gameId] += 1;
        else ConnectionCounts[gameId] = 1;

        return ConnectionCounts[gameId];
    }
    
    private int RemoveViewerFromGame(Guid gameId)
    {
        if (ConnectionCounts.ContainsKey(gameId))
        {
            if (ConnectionCounts[gameId] == 1)
            {
                ConnectionCounts.Remove(gameId);
                return 0;
            }
            else
            {
                ConnectionCounts[gameId] -= 1;
            }
        }

        return ConnectionCounts[gameId];
    }

    private void AddUserToGame(string connectionId, Guid userId)
    {
        if (userId != Guid.Empty) UserConnections[connectionId] = userId;
    }

    private void RemoveUserFromGame(string connectionId, Guid userId)
    {
        if (userId == Guid.Empty) return;
        if (UserConnections.ContainsKey(connectionId)) UserConnections.Remove(connectionId);
    }
}