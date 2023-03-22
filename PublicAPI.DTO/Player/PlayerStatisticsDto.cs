namespace PublicAPI.DTO.Player;

public class PlayerStatisticsDto
{
    public int TotalAttacks { get; set; }
    public int AttackFaults { get; set; }
    public int AttackPoints { get; set; }

    public int Aces { get; set; }
    public int ServeFaults { get; set; }
    
    public int BlockPoints { get; set; }
    public int BlockFaults { get; set; }

    public int TotalReception { get; set; }
    public int ReceptionFaults { get; set; }
    public int PerfectReception { get; set; }
    public int GoodReception { get; set; }
}