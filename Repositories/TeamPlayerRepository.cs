using DAL;
using Domain;
using Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Repositories.Base;

namespace Repositories;

public class TeamPlayerRepository : Repository<TeamPlayers>, ITeamPlayerRepository
{
    public TeamPlayerRepository(AppDbContext appDbContext) : base(appDbContext)
    {
    }

    public TeamPlayers[] GetTeamPlayers(Guid teamId) =>
        LoadAll().Include(tm => tm.Player)
            .ThenInclude(tm => tm.PlayerPhotos)
            .Where(tm => tm.TeamId == teamId).ToArray();
    
    
    public TeamPlayers[] GetForAddingToGame(Guid teamId)
    {
        return LoadAll().Include(t => t.Player)
            .ThenInclude(p => p.PlayerInGames).Where(t => t.TeamId == teamId).ToArray();
    }
}