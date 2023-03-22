using Domain;
using Interfaces.Base;

namespace Interfaces.Repositories;

public interface ITeamPlayerRepository : IBaseRepository<TeamPlayers>
{
    TeamPlayers[] GetTeamPlayers(Guid teamId);

    TeamPlayers[] GetForAddingToGame(Guid teamId);
}