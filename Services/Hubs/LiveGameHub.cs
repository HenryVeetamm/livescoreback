using Exceptions;
using Interfaces.Hubs;
using Interfaces.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using PublicAPI.DTO.PlayerInGame;

namespace Services.Hubs;
[AllowAnonymous]
public class LiveGameHub : Hub<ILiveGameClient>, ILiveGameHub
{
    private readonly IHttpContextService _httpContextService;
    private static HashSet<string> ConnectedIds = new HashSet<string>();
    

    public LiveGameHub(IHttpContextService httpContextService)
    {
        _httpContextService = httpContextService;
    }

    public override async Task OnConnectedAsync()
    {
        var userID = _httpContextService.GetUserId();
        var gameId = GetGameId();
        var connectionId = Context.ConnectionId;
        Console.BackgroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"HUB CONNECTION Got called {Context.ConnectionId} UserId: {userID} GameID:{gameId}");
        ConnectedIds.Add(connectionId);
        await Groups.AddToGroupAsync(connectionId, gameId.ToString());
        
        await Clients.OthersInGroup(gameId.ToString()).PersonJoined(ConnectedIds.Count);
        await Clients.Caller.Connected(ConnectedIds.Count);
            
        await base.OnConnectedAsync();
    }

    public override  Task OnDisconnectedAsync(Exception? exception)
    {
        ConnectedIds.Remove(Context.ConnectionId);
        Console.BackgroundColor = ConsoleColor.Red;
        Console.WriteLine("Disconnected");
        var gameId = GetGameId();

        Clients.OthersInGroup(gameId.ToString()).PersonLeft(ConnectedIds.Count);

        return base.OnDisconnectedAsync(exception);
    }

    private Guid GetGameId()
    {
        var gameId = Context.GetHttpContext().Request.Query["gameId"];
        if (Guid.TryParse(gameId, out Guid result)) return result;
        
        throw new LogicException("GameID not found");
    }

    public Task PlayerDataChanged(Guid teamId, PlayerInGameDto dto)
    {
        throw new NotImplementedException();
    }
}