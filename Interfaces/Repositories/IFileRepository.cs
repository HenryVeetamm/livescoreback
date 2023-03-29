using Domain;
using Interfaces.Base;

namespace Interfaces.Repositories;

public interface IFileRepository : IBaseRepository<Files>
{
    Files GetTeamLogo(Guid teamId);
    Files GetProfilePicture(Guid playerId);
}