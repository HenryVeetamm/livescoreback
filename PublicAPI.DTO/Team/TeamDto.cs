namespace PublicAPI.DTO.Team;

public class TeamDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string HomeStadium { get; set; }

    public string TeamLogoUri { get; set; }
}