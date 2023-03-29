using Constants;
using Domain;
using Exceptions;
using Interfaces.Hubs;
using Interfaces.Repositories;
using Interfaces.Services;
using Microsoft.AspNetCore.Http;
using PublicAPI.DTO.Player;
using Services.Base;

namespace Services;

public class PlayerService : Service, IPlayerService
{
    private readonly IPlayerRepository _playerRepository;
    private readonly ITeamPlayerRepository _teamPlayerRepository;
    private readonly IHttpContextService _httpContextService;
    private readonly ITeamService _teamService;
    private readonly IBlobService _blobService;
    private readonly IFileRepository _fileRepository;
    private readonly IPlayerInGameRepository _playerInGameRepository;
    private readonly ILiveGameHubContext _liveGameHubContext;

    public PlayerService(
        IPlayerRepository playerRepository, 
        ITeamPlayerRepository teamPlayerRepository, 
        IHttpContextService httpContextService, 
        ITeamService teamService,
        IBlobService blobService, 
        IFileRepository fileRepository,
        IPlayerInGameRepository playerInGameRepository,
        ILiveGameHubContext liveGameHubContext)
    {
        _playerRepository = playerRepository;
        _teamPlayerRepository = teamPlayerRepository;
        _httpContextService = httpContextService;
        _teamService = teamService;
        _blobService = blobService;
        _fileRepository = fileRepository;
        _playerInGameRepository = playerInGameRepository;
        _liveGameHubContext = liveGameHubContext;
    }
    public Player AddPlayerToTeam(AddPlayerDto playerDto)
    {
        var team = _teamService.GetMyTeam();
        if (team == null) throw new Exception("Team not found");
        if (team.Id != playerDto.TeamId) throw new LogicException("Team Id's not matching");

        var player = new Player
        {
            FirstName = playerDto.FirstName,
            LastName = playerDto.LastName,
            Position = playerDto.Position,
            DateOfBirth = playerDto.DateOfBirth.ToUniversalTime(),
            ShirtNumber = playerDto.ShirtNumber
        };

        var teamPlayer = new TeamPlayers
        {
            TeamId = team.Id,
            Player = player,
            From = DateTime.Now.ToUniversalTime()
        };
        
        _teamPlayerRepository.Add(teamPlayer);
        _teamPlayerRepository.SaveChanges();

        return player;
    }

    public Player EditPlayer(UpdatePlayerDto playerDto)
    {
        var team = _teamService.GetMyTeam();
        var player = _playerRepository.GetById(playerDto.Id);
        if (team == null ) throw new Exception("Team not found");
        if (team.Id != playerDto.TeamId) throw new LogicException("Team Id's not matching");

        player.FirstName = playerDto.FirstName;
        player.LastName = playerDto.LastName;
        player.Position = playerDto.Position;
        player.DateOfBirth = playerDto.DateOfBirth.ToUniversalTime();
        player.ShirtNumber = playerDto.ShirtNumber;

        _playerRepository.SaveChanges();
        
        return player;
    }

    public Player[] GetTeamPlayers(Guid teamId)
    {
        var teamPlayers = _teamPlayerRepository.GetTeamPlayers(teamId);
        return teamPlayers.Select(tm => tm.Player).ToArray();
    }

    public Player[] GetMyTeamPlayers()
    {
        var team = _teamService.GetMyTeam();
        if (team == null) return null;
        
        var players = GetTeamPlayers(team.Id);
        return players;
    }

    public PlayerInGame[] GetByGameId(Guid gameId, Guid teamId)
    {
        var playersInGame = _playerInGameRepository.GetByGameAndTeamId(teamId, gameId);
        
        return playersInGame;
    }

    public Player[] GetForAddingToGame(Guid gameId)
    {
        var team = _teamService.GetMyTeam();
        if (team == null) return null;
        var teamPlayers = _teamPlayerRepository.GetForAddingToGame(team.Id);
        return teamPlayers.Select(t => t.Player).ToArray();
    }

    public PlayerStatisticsDto GetPlayerStatistics(Guid playerId)
    {
        var playerInGames = _playerInGameRepository.GetAllPlayerGames(playerId);
        var playerStatistics = new PlayerStatisticsDto();

        foreach (var playerInGame in playerInGames)
        {
            playerStatistics.TotalAttacks = playerStatistics.TotalAttacks + playerInGame.AttackFault +
                                            playerInGame.AttackInGame + playerInGame.AttackToPoint;
            playerStatistics.AttackFaults += playerInGame.AttackFault;
            playerStatistics.AttackPoints += playerInGame.AttackToPoint;

            playerStatistics.Aces += playerInGame.Aces;
            playerStatistics.ServeFaults += playerInGame.ServeFaults;

            playerStatistics.BlockPoints += playerInGame.BlockPoint;
            playerStatistics.BlockFaults += playerInGame.BlockFault;

            playerStatistics.TotalReception = playerStatistics.TotalReception + playerInGame.GoodReception +
                                              playerInGame.PerfectReception + playerInGame.ReceptionFault;
            playerStatistics.ReceptionFaults += playerInGame.ReceptionFault;
            playerStatistics.PerfectReception += playerInGame.PerfectReception;
            playerStatistics.GoodReception += playerInGame.GoodReception;
        }

        return playerStatistics;
    }

    public void UploadProfilePicture(IFormFile file, Guid playerId)
    {
       CanManageGivenPlayer(playerId);
       
       var profilePicture = _fileRepository.GetProfilePicture(playerId);
       if (profilePicture != null) _fileRepository.Delete(profilePicture);
       
        using (var memoryStream = new MemoryStream())
        {
            file.CopyTo(memoryStream);
            memoryStream.Position = 0;
            var response = _blobService.UploadMemoryStream(file.FileName, memoryStream, file.ContentType, BlobStorageContainers.PROFILE_PICTURES);

            var newFile = new Files
            {
                MimeType = file.ContentType,
                FileName = file.Name,
                AbsoluteUri = response,
                PlayerId = playerId
            };
            
            _fileRepository.Add(newFile);
            _fileRepository.SaveChanges();
        }
    }

    private void CanManageGivenPlayer(Guid playerId)
    {
        var givenUserTeam = _teamService.GetMyTeam();
        if (givenUserTeam == null) throw new Exception("Sisselogitud kasutajat ei leitud");

        var teamPlayer = _teamPlayerRepository.GetByPlayerId(givenUserTeam.Id, playerId);
        if (teamPlayer == null) throw new Exception("Puudub õigus muuta");
    }
}