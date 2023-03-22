using Domain.Base;
using Domain.Enums;

namespace Domain;

//Creator is home team.
public class Game : BaseEntity
{

    public Guid HomeTeamId { get; set; }
    public Team HomeTeam { get; set; }
    public Guid? AwayTeamId { get; set; }
    public Team? AwayTeam { get; set; }
    
    public string? AwayTeamName { get; set; }
    
    public int? HomeTeamSetWins { get; set; }
    public int? AwayTeamSetWins { get; set; }

    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public DateTime ScheduledTime { get; set; }
    
    public string Location { get; set; }

    public EGameStatus GameStatus { get; set; }
    public EGameType GameType { get; set; }
    
    public bool? Confirmed { get; set; }
    
    public ICollection<Files>? GeneratedStatistics { get; set; }
    public ICollection<Set>? Sets { get; set; }
    public ICollection<PlayerInGame>? PlayerInGames { get; set; }
}