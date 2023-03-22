using AutoMapper;
using Interfaces.Converters;
using PublicAPI.DTO.Player;

namespace Converters.Player;

public class PlayerConverter : Converter<Domain.Player, PlayerDto>, IPlayerConverter
{
    public PlayerConverter(IMapper mapper) : base(mapper)
    {
    }

    public override PlayerDto Convert(Domain.Player entity)
    {
        if (entity == null) return null;
        
        return new PlayerDto
        {
            Id = entity.Id,
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            DateOfBirth = entity.DateOfBirth,
            Position = entity.Position,
            ShirtNumber = entity.ShirtNumber,
            ProfileAbsoulteUri = entity.PlayerPhotos?.FirstOrDefault()?.AbsoluteUri
        };
    }
}