using DAL;
using Domain;
using Domain.Enums;
using Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Repositories.Base;

namespace Repositories;

public class GameRepository : Repository<Game>, IGameRepository
{
    public GameRepository(AppDbContext appDbContext) : base(appDbContext)
    {
    }

    public Game[] GetGames(int pageNumber, int pageSize)
    {
        var skip = pageNumber <= 1 ? 0 : (pageNumber - 1) * pageSize;
        return LoadAll()
            .Include(g => g.HomeTeam)
            .ThenInclude(g => g.TeamPhotos)
            .Include(g => g.AwayTeam)
            .ThenInclude(g => g.TeamPhotos)
            .OrderByDescending(x => x.GameStatus == EGameStatus.Started)
            .ThenByDescending(x => x.GameStatus == EGameStatus.NotStarted)
            .ToArray();
    }

    public Game GetGameById(Guid gameId)
    {
        var game = LoadAll()
            .Include(t => t.HomeTeam)
            .ThenInclude(ht => ht.TeamPhotos)
            .Include(t => t.AwayTeam)
            .ThenInclude(t => t.TeamPhotos)
            .FirstOrDefault(g => g.Id == gameId);

        return game;
    }

    public Game[] GetGamesByTeamId(Guid teamId)
    {
        return LoadAll()
            .Include(g => g.HomeTeam)
            .ThenInclude(g => g.TeamPhotos)
            .Include(g => g.AwayTeam)
            .ThenInclude(g => g.TeamPhotos)
            .Where(g => g.HomeTeamId == teamId || g.AwayTeamId.HasValue && g.AwayTeamId == teamId)
            .OrderByDescending(x => x.GameStatus == EGameStatus.Started)
            .ThenByDescending(x => x.GameStatus == EGameStatus.NotStarted)
            .ToArray();
        
    }
}