using AutoMapper;
using Domain.Enums;
using Interfaces.Converters;
using PublicAPI.DTO.Game;

namespace Converters.Game;

public class GameConverter : Converter<Domain.Game, GameDto>, IGameConverter
{
    private readonly ITeamConverter _teamConverter;

    public GameConverter(IMapper mapper, ITeamConverter teamConverter) : base(mapper)
    {
        _teamConverter = teamConverter;
    }

    public override GameDto Convert(Domain.Game entity)
    {
        if (entity == null) return null;

        return new GameDto
        {
            Id = entity.Id,
            HomeTeam = _teamConverter.Convert(entity.HomeTeam),
            AwayTeam = _teamConverter.Convert(entity.AwayTeam),
            AwayTeamName = entity.AwayTeamName,
            HomeTeamSetWins = entity.HomeTeamSetWins,
            AwayTeamSetWins = entity.AwayTeamSetWins,
            ScheduledTime = entity.ScheduledTime,
            Location = entity.Location,
            GameStatus = entity.GameStatus,
            GameType = entity.GameType,
            Confirmed = entity.Confirmed,
            IsGameLive = entity.GameStatus == EGameStatus.Started,
            StartTime = entity.StartTime,
            EndTime = entity.EndTime,
            
        };
    }
}