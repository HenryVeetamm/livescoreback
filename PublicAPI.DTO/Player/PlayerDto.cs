using Domain.Enums;

namespace PublicAPI.DTO.Player;

public class PlayerDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public EPlayerPosition Position { get; set; }
    public int ShirtNumber { get; set; }

    public string? ProfileAbsoulteUri { get; set; }
}
