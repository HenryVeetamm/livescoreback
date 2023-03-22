using Domain.Enums;
using PublicAPI.DTO.Team;

namespace PublicAPI.DTO.Game;

public class GameDto
{
    public Guid Id { get; set; }
    public TeamDto HomeTeam { get; set; }
    public TeamDto? AwayTeam { get; set; }
    public string? AwayTeamName { get; set; }
    public int? HomeTeamSetWins { get; set; }
    public int? AwayTeamSetWins { get; set; }
    
    public DateTime ScheduledTime { get; set; }
    
    public string Location { get; set; }

    public EGameStatus GameStatus { get; set; }
    public EGameType GameType { get; set; }
    public bool? Confirmed { get; set; }
    public bool IsGameLive { get; set; }

    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
}