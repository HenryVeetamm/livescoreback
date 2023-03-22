using DAL;
using Domain;
using Interfaces.Repositories;
using Repositories.Base;

namespace Repositories;

public class PlayerRepository: Repository<Player>, IPlayerRepository
{
    public PlayerRepository(AppDbContext appDbContext) : base(appDbContext)
    {
    }
}