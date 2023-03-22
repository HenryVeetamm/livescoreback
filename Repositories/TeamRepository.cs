using DAL;
using Domain;
using Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Repositories.Base;

namespace Repositories;

public class TeamRepository : Repository<Team>, ITeamRepository
{
    public TeamRepository(AppDbContext appDbContext) : base(appDbContext) { }

    public bool HasTeam(Guid userId) => LoadAll().Any(t => t.UserId == userId);

    
    public Team GetByIdAndUserId(Guid teamId, Guid userId) =>
        LoadAll().FirstOrDefault(t => t.Id == teamId && t.UserId == userId);

    public Team GetByUserId(Guid userId) => LoadAll()
        .Include(t => t.TeamPhotos)
        .FirstOrDefault(t => t.UserId == userId);

    public Team[] GetTeamsForSearch(Guid excludeMyTeam) => LoadAll().Where(t => t.Id != excludeMyTeam).ToArray();
    
    public Team[] GetTeamsWithPictures()
    {
        return LoadAll().Include(x => x.TeamPhotos).ToArray();
    }

    public Team GetTeamById(Guid teamId)
    {
        return LoadAll()
            .Include(t => t.TeamPhotos)
            .FirstOrDefault(t => t.Id == teamId);
    }
}