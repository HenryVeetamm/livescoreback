using Domain;
using Interfaces.Base;

namespace Interfaces.Repositories;

public interface ITeamRepository : IBaseRepository<Team>
{
    bool HasTeam(Guid userId);
    Team GetByIdAndUserId(Guid teamId, Guid userId);
    Team GetByUserId(Guid userId);
    Team[] GetTeamsForSearch(Guid excludeMyTeam);
    Team[] GetTeamsWithPictures();
    Team GetTeamById(Guid teamId);
}