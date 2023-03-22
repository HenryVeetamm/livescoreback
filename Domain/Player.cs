using System.Collections;
using Domain.Base;
using Domain.Enums;

namespace Domain;

public class Player : BaseEntity
{
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public EPlayerPosition Position { get; set; }
    
    public DateTime DateOfBirth { get; set; }

    public int ShirtNumber { get; set; }

    public ICollection<PlayerInGame>? PlayerInGames { get; set; }
    public ICollection<TeamPlayers>? TeamPlayers { get; set; }
    public ICollection<Files>? PlayerPhotos { get; set; }
    
}