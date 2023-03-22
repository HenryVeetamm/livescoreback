namespace PublicAPI.DTO.Set;

public class SetDto
{
    public Guid Id { get; set; }
    public Guid GameId { get; set; }
    public bool IsActive { get; set; }
    public int HomeTeamScore { get; set; }
    public int AwayTeamScore { get; set; }

    public int SetIndex { get; set; }
}