namespace Interfaces.Converters;

public interface IConverter<TEntity, TDto>
{
    TDto Convert(TEntity entity);
    TDto[] ConvertAll(TEntity[] entities);
}