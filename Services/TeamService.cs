using Constants;
using Domain;
using Exceptions;
using Interfaces.Repositories;
using Interfaces.Services;
using Microsoft.AspNetCore.Http;
using PublicAPI.DTO.Team;
using Services.Base;

namespace Services;

public class TeamService: Service, ITeamService
{
    private readonly ITeamRepository _teamRepository;
    private readonly IHttpContextService _httpContextService;
    private readonly IBlobService _blobService;
    private readonly IFileRepository _fileRepository;

    public TeamService(
        ITeamRepository teamRepository, 
        IHttpContextService httpContextService,
        IBlobService blobService,
        IFileRepository fileRepository)
    {
        _teamRepository = teamRepository;
        _httpContextService = httpContextService;
        _blobService = blobService;
        _fileRepository = fileRepository;
    }
    public Team AddTeam(CreateTeamDto teamDto)
    {
        var userId = _httpContextService.GetUserId();
        var hasTeam = _teamRepository.HasTeam(userId);

        if (hasTeam) Console.WriteLine("User already has a team");

        var team = new Team
        {
            UserId = userId,
            Name = teamDto.Name,
            HomeStadium = teamDto.HomeStadium
        };
        
        _teamRepository.Add(team);
        _teamRepository.SaveChanges();
        
        return team;
    }

    public Team UpdateTeam(UpdateTeamDto teamDto)
    {
        var userId = _httpContextService.GetUserId();
        var team = _teamRepository.GetByIdAndUserId(teamDto.Id, userId);

        if (team == null) throw new LogicException("Teie võistkonda ei leitud");

        team.Name = teamDto.Name;
        team.HomeStadium = teamDto.HomeStadium;
        _teamRepository.SaveChanges();

        return team;
    }

    public Team GetMyTeam()
    {
        var userId = _httpContextService.GetUserId();
        var team = _teamRepository.GetByUserId(userId);
        return team;
    }

    public void UploadTeamLogo(IFormFile file, Guid teamId)
    {
        if (teamId == null) throw new LogicException("Võistkonda ei leitud");

        var teamLogo = _fileRepository.GetTeamLogo(teamId);
        if (teamLogo != null) _fileRepository.Delete(teamLogo); 
        
        using (var memoryStream = new MemoryStream())
        {
            file.CopyTo(memoryStream);
            memoryStream.Position = 0;
            var response = _blobService.UploadMemoryStream(file.FileName, memoryStream, file.ContentType, BlobStorageContainers.TEAM_LOGOS);
            
            if (response == null) return;
            
            var newFile = new Files
            {
                MimeType = file.ContentType,
                FileName = file.Name,
                AbsoluteUri = response,
                TeamId = teamId
            };
            
            _fileRepository.Add(newFile);
            _fileRepository.SaveChanges();
        }    
    }

    public Team[] GetTeams()
    {
        var team = GetMyTeam();
        if (team == null) throw new LogicException("Teie võistkonda ei leitud");

        var teams = _teamRepository.GetTeamsForSearch(team.Id);
        return teams;
    }
}