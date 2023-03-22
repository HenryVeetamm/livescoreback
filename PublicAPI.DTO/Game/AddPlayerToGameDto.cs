namespace PublicAPI.DTO.Game;

public class AddPlayerToGameDto
{
    public Guid PlayerId { get; set; }
    public Guid GameId { get; set; }
    public bool Active { get; set; }
}