using AutoMapper;
using Interfaces.Converters;
using PublicAPI.DTO.Player;

namespace Converters.Player;

public class AddPlayerToGameConverter: Converter<Domain.Player, AddPlayerInGameDto>, IAddPlayerToGameConverter
{
    public AddPlayerToGameConverter(IMapper mapper) : base(mapper)
    {
    }

    public AddPlayerInGameDto Convert(Domain.Player entity, Guid gameId)
    {
        return new AddPlayerInGameDto
        {
            Id = entity.Id,
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            ShirtNumber = entity.ShirtNumber,
            AlreadyAdded = entity.PlayerInGames != null && entity.PlayerInGames.Any(pig => pig.GameId == gameId)
        };
    }

    public AddPlayerInGameDto[] ConvertAll(Domain.Player[] entities, Guid gameId)
    {
        var res = entities.Select(x => Convert(x, gameId)).Where(e => e != null).ToArray();
        return res;
    }
}