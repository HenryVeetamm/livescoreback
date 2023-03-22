using Domain.Enums;

namespace PublicAPI.DTO.Game;

public class ManageGameScoreDto
{
    public Guid GameId { get; set; }
    public EMethod Method { get; set; }
    public ETeam Team { get; set; }
}