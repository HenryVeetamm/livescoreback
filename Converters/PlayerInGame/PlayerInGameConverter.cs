using AutoMapper;
using Interfaces.Converters;
using PublicAPI.DTO.Player;
using PublicAPI.DTO.PlayerInGame;

namespace Converters.PlayerInGame;

public class PlayerInGameConverter : Converter<Domain.PlayerInGame, PlayerInGameDto>, IPlayerInGameConverter
{
    private readonly IPlayerConverter _playerConverter;

    public PlayerInGameConverter(IMapper mapper, IPlayerConverter playerConverter ) : base(mapper)
    {
        _playerConverter = playerConverter;
    }

    public override PlayerInGameDto Convert(Domain.PlayerInGame entity)
    {
        return new PlayerInGameDto
        {
            Id = entity.Id,
            Player = _playerConverter.Convert(entity.Player),
            Aces = entity.Aces,
            ServeFaults = entity.ServeFaults,
            AttackInGame = entity.AttackInGame,
            AttackToPoint = entity.AttackToPoint,
            AttackFault = entity.AttackFault,
            BlockFault = entity.BlockFault,
            BlockPoint = entity.BlockPoint,
            PerfectReception = entity.PerfectReception,
            GoodReception = entity.GoodReception,
            ReceptionFault = entity.ReceptionFault
        };
    }
}