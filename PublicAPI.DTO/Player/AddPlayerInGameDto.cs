namespace PublicAPI.DTO.Player;

public class AddPlayerInGameDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int ShirtNumber { get; set; }
    public bool AlreadyAdded { get; set; }
}