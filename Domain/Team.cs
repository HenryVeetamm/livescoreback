using System.Collections;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Base;
using Domain.Identity;

namespace Domain;

public class Team : BaseEntity
{
    public Guid UserId { get; set; }
    public AppUser User { get; set; }

    public string Name { get; set; }
    public string HomeStadium { get; set; }
    
    
    [InverseProperty(nameof(Game.HomeTeam))]
    public ICollection<Game>? HomeTeamGames { get; set; }
    
    [InverseProperty(nameof(Game.AwayTeam))]
    public ICollection<Game>? AwayTeamGames { get; set; }

    public ICollection<Files>? TeamPhotos { get; set; }

}