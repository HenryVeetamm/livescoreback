using PublicAPI.DTO.Game;
using PublicAPI.DTO.PlayerInGame;
using PublicAPI.DTO.Set;

namespace Interfaces.Hubs;

public interface ILiveGameClient
{
    Task PersonJoined(int data);
    Task PersonLeft(int data);
    Task Connected(int data);
    Task AddPointToGame();
    Task PlayerDataChanged(Guid teamId, Guid gameId, PlayerInGameDto dto, ManagePlayerPointsDto playerDataChangesDto);
    Task GameScoreChanged(Guid gameId, SetDto set);
    Task GameStarted(GameDto game, SetDto setDto);
    Task StartNewSet(Guid gameId, SetDto set);
    Task EndGame(GameDto game);

    Task PlayersChanged(Guid teamId, PlayerInGameDto[] players);
}