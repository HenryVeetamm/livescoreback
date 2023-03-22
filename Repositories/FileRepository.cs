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
}