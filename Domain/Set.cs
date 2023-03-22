using Domain.Base;

namespace Domain;

public class Set : BaseEntity
{
    public Guid GameId { get; set; }
    public Game Game { get; set; }

    public bool IsActive { get; set; }
    public int HomeTeamScore { get; set; }
    public int AwayTeamScore { get; set; }
    public int SetIndex { get; set; }
}