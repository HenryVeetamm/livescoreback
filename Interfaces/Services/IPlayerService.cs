using Domain;
using Interfaces.Base;
using Microsoft.AspNetCore.Http;
using PublicAPI.DTO.Player;

namespace Interfaces.Services;

public interface IPlayerService : IBaseService
{
    Player AddPlayerToTeam(AddPlayerDto playerDto);
    Player EditPlayer(UpdatePlayerDto playerDto);
    Player[] GetTeamPlayers(Guid teamId);
    Player[] GetMyTeamPlayers();
    void UploadProfilePicture(IFormFile file, Guid playerId);
    PlayerInGame[] GetByGameId(Guid gameId, Guid teamId);
    Player[] GetForAddingToGame(Guid gameId);
    PlayerStatisticsDto GetPlayerStatistics(Guid playerId);
}