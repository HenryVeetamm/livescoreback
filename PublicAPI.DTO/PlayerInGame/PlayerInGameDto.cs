using PublicAPI.DTO.Player;

namespace PublicAPI.DTO.PlayerInGame;

public class PlayerInGameDto
{
    public Guid Id { get; set; }
    public PlayerDto Player { get; set; }
    
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