using Domain.Base;

namespace Domain;

/// <summary>
/// Holds the main information about player in the game. All the statistics points are presented here.
/// Total
/// </summary>
public class PlayerInGame : BaseEntity
{
    public Guid GameId { get; set; }
    public Game Game { get; set; }

    public Guid PlayerId { get; set; }
    public Player Player { get; set; }

    public int Aces { get; set; }
    public int ServeFaults { get; set; }

    public int AttackInGame { get; set; }
    public int AttackToPoint { get; set; }
    public int AttackFault { get; set; }

    public int BlockPoint { get; set; }
    public int BlockFault { get; set; }

    /// <summary>
    /// Perf
    /// </summary>
    public int PerfectReception { get; set; }
    
    /// <summary>
    /// In game
    /// </summary>
    public int GoodReception { get; set; }
   
    /// <summary>
    /// Fault
    /// </summary>
    public int ReceptionFault { get; set; }
    
    
}