using Domain;
using Interfaces.Base;
using Microsoft.AspNetCore.Http;
using PublicAPI.DTO.Team;

namespace Interfaces.Services;

public interface ITeamService : IBaseService
{
    Team AddTeam(CreateTeamDto teamDto);
    Team UpdateTeam(UpdateTeamDto teamDto);
    Team GetMyTeam();
    void UploadTeamLogo(IFormFile file, Guid teamId);
    Team[] GetTeams();
    
}