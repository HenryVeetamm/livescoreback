using Domain;
using PublicAPI.DTO.Player;
using PublicAPI.DTO.PlayerInGame;

namespace Interfaces.Hubs;

public interface ILiveGameHubContext
{
    Task AddPointToGameAsync(Guid gameId);
    
    Task PlayerDataChangedAsync(Guid teamId, Guid gameId, PlayerInGame playerInGame, ManagePlayerPointsDto dto);

    Task GameScoreChangedAsync(Guid gameId, Set set);

    Task GameStartedAsync(Game game, Set setDto);
    Task StartNewSetAsync(Guid gameId, Set set);
    Task EndGameAsync(Guid gameId, Game game);

    Task PlayersChangedAsync(Guid gameId, Guid teamId, PlayerInGame[] playerInGameDtos);
}