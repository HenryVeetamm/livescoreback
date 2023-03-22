using AutoMapper;
using Interfaces.Converters;
using PublicAPI.DTO.Team;

namespace Converters.Team;

public class TeamConverter : Converter<Domain.Team, TeamDto>, ITeamConverter
{
    public TeamConverter(IMapper mapper) : base(mapper)
    {
    }

    public override TeamDto Convert(Domain.Team? entity)
    {
        if (entity == null) return null;
        
        return new TeamDto
        {
            Id = entity.Id,
            Name = entity.Name,
            HomeStadium = entity.HomeStadium,
            TeamLogoUri = entity.TeamPhotos != null && entity.TeamPhotos.Any() ? entity.TeamPhotos.FirstOrDefault().AbsoluteUri : null
        };
    }
}