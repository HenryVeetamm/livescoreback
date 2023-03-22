using Azure.Core;
using DAL;
using Domain;
using Interfaces.Repositories;
using Repositories.Base;

namespace Repositories;

public class SetRepository: Repository<Set>, ISetRepository
{
    public SetRepository(AppDbContext appDbContext) : base(appDbContext)
    {
    }

    public Set GetCurrentGameSet(Guid gameId)
    {
        var result = LoadAll().FirstOrDefault(x => x.GameId == gameId && x.IsActive);
        return result;
    }

    public Set[] GetCompletedSetsByGameId(Guid gameId)
    {
        var result = LoadAll().Where(x => x.GameId == gameId && x.IsActive == false).ToArray();
        return result;
    }
}