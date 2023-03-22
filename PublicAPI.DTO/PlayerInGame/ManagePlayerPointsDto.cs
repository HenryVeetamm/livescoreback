using Domain.Enums;

namespace PublicAPI.DTO.PlayerInGame;

public class ManagePlayerPointsDto
{
    public Guid PlayerInGameId { get; set; }
    public EMethod Method { get; set; }
    public ECategoryResult CategoryResult { get; set; }
    public EPointCategory Category { get; set; }
}