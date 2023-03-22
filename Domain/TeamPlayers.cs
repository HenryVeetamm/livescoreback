using Domain.Base;

namespace Domain;

public class TeamPlayers : BaseEntity
{
    public Guid TeamId { get; set; }
    public Team Team { get; set; }

    public Guid PlayerId { get; set; }
    public Player Player { get; set; }

    public DateTime From { get; set; }
    
    /// <summary>
    /// Is still in this team
    /// </summary>
    public DateTime? Until { get; set; }
    
}