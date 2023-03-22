using Domain.Base;

namespace Domain;

/// <summary>
/// Holds file information
/// GameId is for generated statistics
/// PlayerId is for Player profile phtotos
/// TeamId is for Team Photos
/// </summary>

public class Files : BaseEntity
{
    public string MimeType { get; set; }

    public string FileName { get; set; }

    public string AbsoluteUri { get; set; }

    public Guid? PlayerId { get; set; }
    public Player? Player { get; set; }

    public Guid? GameId { get; set; }
    public Game? Game { get; set; }

    public Guid? TeamId { get; set; }
    public Team? Team { get; set; }
    
}