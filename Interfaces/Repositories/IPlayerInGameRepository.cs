using Domain;
using Interfaces.Base;

namespace Interfaces.Repositories;

public interface IPlayerInGameRepository : IBaseRepository<PlayerInGame>
{
    PlayerInGame GetByPlayerAndGameId(Guid playerId, Guid gameId);

    PlayerInGame[] GetByGameAndTeamId(Guid teamId, Guid gameId);

    PlayerInGame GetWithPlayer(Guid playerInGameId);

    PlayerInGame[] GetAllPlayerGames(Guid playerId);

    PlayerInGame[] GetAllPlayersInGameByGameId(Guid gameId);
}