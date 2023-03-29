using DAL;
using Domain;
using Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Repositories.Base;

namespace Repositories;

public class PlayerInGameRepository: Repository<PlayerInGame>, IPlayerInGameRepository
{
    public PlayerInGameRepository(AppDbContext appDbContext) : base(appDbContext)
    {
    }

    public PlayerInGame GetByPlayerAndGameId(Guid playerId, Guid gameId)
    {
        return LoadAll().FirstOrDefault(x => x.PlayerId == playerId && x.GameId == gameId);
    }

    public PlayerInGame[] GetByGameAndTeamId(Guid teamId, Guid gameId)
    {
        return LoadAll()
            .Include(p => p.Player)
            .Include(p => p.Game)
            .ThenInclude(p => p.HomeTeam)
            .Include(p => p.Game)
            .ThenInclude(p => p.AwayTeam)
            .Where(pig => pig.GameId == gameId)
            .Where(pig => pig.Player.TeamPlayers.Any(t => t.TeamId == teamId))
            .ToArray();
    }

    public PlayerInGame GetWithPlayer(Guid playerInGameId)
    {
        return LoadAll().Include(pig => pig.Player).FirstOrDefault(pig => pig.Id == playerInGameId);
    }

    public PlayerInGame[] GetAllPlayerGames(Guid playerId)
    {
        var playerInGameDtos = LoadAll().Where(x => x.PlayerId == playerId);
        return playerInGameDtos.ToArray();
    }

    public PlayerInGame[] GetAllPlayersInGameByGameId(Guid gameId)
    {
        return LoadAll().Where(pig => pig.GameId == gameId).ToArray();
    }
}