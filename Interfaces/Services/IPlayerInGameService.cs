using Domain;
using Interfaces.Base;
using PublicAPI.DTO.PlayerInGame;

namespace Interfaces.Services;

public interface IPlayerInGameService : IBaseService
{
    PlayerInGame ManagePlayerResult(ManagePlayerPointsDto dto, Guid teamId, Guid gameId);
}