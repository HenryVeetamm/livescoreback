using Domain.Base;

namespace Interfaces.Base;

public interface IBaseRepository<T> where T : IBaseEntity
{
    T[] GetAll();

    T GetById(Guid id);

    void Add(T entity);

    void Update(T entity);

    void Delete(T entity);

    void SaveChanges();
}