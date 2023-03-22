using Domain.Enums;

namespace PublicAPI.DTO.Game;

public class AddGameDto
{
    public Guid? AwayTeamId { get; set; }
    public string? AwayTeamName { get; set; }

    public string Location { get; set; }
    
    public DateTime ScheduledTime { get; set; }
    
    public EGameType GameType { get; set; }
}