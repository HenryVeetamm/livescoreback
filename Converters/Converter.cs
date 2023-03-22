using AutoMapper;
using Interfaces.Converters;

namespace Converters;

public class Converter<TEntity, TDto> : IConverter<TEntity, TDto>
{
    private readonly IMapper _mapper;

    public Converter(IMapper mapper)
    {
        _mapper = mapper;
    }

    public virtual TDto Convert(TEntity entity) => _mapper.Map<TDto>(entity);

    public virtual TDto[] ConvertAll(TEntity[] entities) => entities?.Select(Convert).Where(e => e != null).ToArray();
}