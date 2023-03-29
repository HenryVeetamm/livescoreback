using DAL;
using Domain;
using Interfaces.Repositories;
using Repositories.Base;

namespace Repositories;

public class FileRepository: Repository<Files>, IFileRepository 
{
    public FileRepository(AppDbContext appDbContext) : base(appDbContext)
    {
    }

    public Files GetTeamLogo(Guid teamId)
    {
        return LoadAll().FirstOrDefault(f => f.TeamId == teamId);
    }

    public Files GetProfilePicture(Guid playerId)
    {
        return LoadAll().FirstOrDefault(f => f.PlayerId == playerId);
    }
}