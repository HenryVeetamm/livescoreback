using Domain;
using Interfaces.Base;

namespace Interfaces.Repositories;

public interface ISetRepository : IBaseRepository<Set>
{
    Set GetCurrentGameSet(Guid gameId);
    Set[] GetCompletedSetsByGameId(Guid gameId);
}