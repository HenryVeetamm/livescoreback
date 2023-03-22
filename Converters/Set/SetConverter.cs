using AutoMapper;
using Interfaces.Converters;
using PublicAPI.DTO.Set;

namespace Converters.Set;

public class SetConverter : Converter<Domain.Set, SetDto>, ISetConverter
{
    public SetConverter(IMapper mapper) : base(mapper)
    {
    }

    public override SetDto Convert(Domain.Set entity)
    {
        if (entity == null) return null;
        return new SetDto
        {
            Id = entity.Id,
            GameId = entity.GameId,
            IsActive = entity.IsActive,
            HomeTeamScore = entity.HomeTeamScore,
            AwayTeamScore = entity.AwayTeamScore,
            SetIndex = entity.SetIndex
        };
    }
}