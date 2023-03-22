using DAL;
using Domain.Base;
using Interfaces.Base;
using Microsoft.EntityFrameworkCore;

namespace Repositories.Base;

public class Repository<T> : IBaseRepository<T> where T : BaseEntity
{
    private readonly AppDbContext _appDbContext;
    private readonly DbSet<T> _dbSet;

    public Repository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
        _dbSet = appDbContext.Set<T>();
    }

    public T[] GetAll() => LoadAll().ToArray();

    public T GetById(Guid id) => LoadAll().FirstOrDefault(e => e.Id == id);

    public void Add(T entity) => _dbSet.Add(entity);

    public void Update(T entity) => _dbSet.Update(entity);

    public void Delete(T entity) => _dbSet.Remove(entity);

    public void SaveChanges() => _appDbContext.SaveChanges();
    
    internal IQueryable<T> LoadAll() => _dbSet.AsQueryable();
}