using Domain;
using Interfaces.Base;
using PublicAPI.DTO.Game;

namespace Interfaces.Services;

public interface IGameService : IBaseService
{
    Game AddGame(AddGameDto addGameDto);
    void AddPlayerToGame(AddPlayerToGameDto dto);
    Set ManageGameScore(ManageGameScoreDto dto);
    Set StartGame(Guid gameId);
    Set StartNewSet(Guid gameId);
    Set GetCurrentSet(Guid gameId);
    Game[] GetMyGames();
    Game EndGame(Guid gameId);
}