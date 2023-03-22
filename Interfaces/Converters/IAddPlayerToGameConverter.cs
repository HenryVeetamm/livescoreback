using Domain;
using PublicAPI.DTO.Game;
using PublicAPI.DTO.Player;

namespace Interfaces.Converters;

public interface IAddPlayerToGameConverter: IConverter<Player, AddPlayerInGameDto>
{
    AddPlayerInGameDto Convert(Domain.Player entity, Guid gameId);
    AddPlayerInGameDto[] ConvertAll(Domain.Player[] entities, Guid gameId);
}