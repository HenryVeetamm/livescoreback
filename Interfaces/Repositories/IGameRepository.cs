using Domain;
using Interfaces.Base;

namespace Interfaces.Repositories;

public interface IGameRepository: IBaseRepository<Game>
{
    Game[] GetGames(int pageNumber, int pageSize);
    Game GetGameById(Guid gameId);
    Game[] GetGamesByTeamId(Guid teamId);
}