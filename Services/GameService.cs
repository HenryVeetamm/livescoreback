using Domain;
using Domain.Enums;
using Exceptions;
using Interfaces.Hubs;
using Interfaces.Repositories;
using Interfaces.Services;
using Microsoft.AspNetCore.SignalR;
using PublicAPI.DTO.Game;
using Services.Base;
using Services.Hubs;

namespace Services;

public class GameService : Service, IGameService
{
    private readonly ITeamService _teamService;
    private readonly IGameRepository _gameRepository;
    private readonly ILiveGameHubContext _liveGameHubContext;
    private readonly IPlayerInGameRepository _playerInGameRepository;
    private readonly ISetRepository _setRepository;

    public GameService(
        ITeamService teamService,
        IGameRepository gameRepository,
        ILiveGameHubContext liveGameHubContext,
        IPlayerInGameRepository playerInGameRepository,
        ISetRepository setRepository)
    {
        _teamService = teamService;
        _gameRepository = gameRepository;
        _liveGameHubContext = liveGameHubContext;
        _playerInGameRepository = playerInGameRepository;
        _setRepository = setRepository;
    }

    public Game AddGame(AddGameDto addGameDto)
    {
        var myTeam = _teamService.GetMyTeam();
        if (myTeam == null) throw new Exception("Team not found");

        var game = new Game
        {
            HomeTeamId = myTeam.Id,
            AwayTeamId = addGameDto.AwayTeamId,
            AwayTeamName = addGameDto.AwayTeamName,
            ScheduledTime = addGameDto.ScheduledTime,
            Location = addGameDto.Location,
            GameStatus = EGameStatus.NotStarted,
            GameType = addGameDto.GameType
        };

        _gameRepository.Add(game);
        _gameRepository.SaveChanges();


        return game;
    }

    public void AddPlayerToGame(AddPlayerToGameDto dto)
    {
        //Check if game is active;
        var myTeam = _teamService.GetMyTeam();
        if (myTeam == null) throw new LogicException("Team not found");

        if (!dto.Active)
        {
            var existing = _playerInGameRepository.GetByPlayerAndGameId(dto.PlayerId, dto.GameId);
            if (existing != null) _playerInGameRepository.Delete(existing);
        }
        else
        {
            var playerInGame = new PlayerInGame
            {
                GameId = dto.GameId,
                PlayerId = dto.PlayerId
            };
            _playerInGameRepository.Add(playerInGame);
        }

        _playerInGameRepository.SaveChanges();

        var playerInTeam = _playerInGameRepository.GetByGameAndTeamId(myTeam.Id, dto.GameId);
        _liveGameHubContext.PlayersChangedAsync(dto.GameId, myTeam.Id, playerInTeam);
    }

    public Set ManageGameScore(ManageGameScoreDto dto)
    {
        var set = _setRepository.GetCurrentGameSet(dto.GameId);
        if (set == null) throw new LogicException("Geimi ei leitud");

        if (dto.Method == EMethod.Increment) Increment(set, dto);
        if (dto.Method == EMethod.Decrement) Decrement(set, dto);

        _setRepository.SaveChanges();

        _liveGameHubContext.GameScoreChangedAsync(set.GameId, set);
        return set;
    }

    private void Increment(Set set, ManageGameScoreDto dto)
    {
        if (dto.Team == ETeam.HomeTeam) set.HomeTeamScore += 1;
        if (dto.Team == ETeam.AwayTeam) set.AwayTeamScore += 1;
    }

    private void Decrement(Set set, ManageGameScoreDto dto)
    {
        if (dto.Team == ETeam.HomeTeam && set.HomeTeamScore > 0) set.HomeTeamScore -= 1;
        if (dto.Team == ETeam.AwayTeam && set.AwayTeamScore > 0) set.AwayTeamScore -= 1;

    }

    public Set StartGame(Guid gameId)
    {
        var game = _gameRepository.GetById(gameId);
        if (game.GameStatus != EGameStatus.NotStarted) throw new LogicException("Mäng on juba alanud");

        game.GameStatus = EGameStatus.Started;
        game.StartTime = DateTime.Now.ToUniversalTime();
        _gameRepository.SaveChanges();

        var newSet = new Set
        {
            GameId = gameId,
            IsActive = true,
            SetIndex = 1
        };
        _liveGameHubContext.GameStartedAsync(game, newSet);

        _setRepository.Add(newSet);
        _setRepository.SaveChanges();


        return newSet;
    }

    public Set StartNewSet(Guid gameId)
    {
        var set = _setRepository.GetCurrentGameSet(gameId);
        if (set == null) throw new LogicException("Aktiivset geimi ei leitud");

        set.IsActive = false;
        var newSet = new Set
        {
            GameId = gameId,
            IsActive = true,
            SetIndex = set.SetIndex + 1
        };
        _setRepository.Add(newSet);
        _setRepository.SaveChanges();

        _liveGameHubContext.StartNewSetAsync(gameId, newSet);
        return newSet;
    }

    public Set GetCurrentSet(Guid gameId)
    {
        var game = _gameRepository.GetById(gameId);
        var set = _setRepository.GetCurrentGameSet(gameId);
        if (game.GameStatus != EGameStatus.Started) return null;
        if (set == null) throw new LogicException("Aktiivset geimi ei leitud");

        return set;
    }

    public Game[] GetMyGames()
    {
        var myTeam = _teamService.GetMyTeam();
        if (myTeam == null) return null;
        return _gameRepository.GetGamesByTeamId(myTeam.Id);

    }

    public Game EndGame(Guid gameId)
    {
        var game = _gameRepository.GetGameById(gameId);
        var set = _setRepository.GetCurrentGameSet(gameId);
        if (set == null) throw new LogicException("Aktiivset geimi ei leitud");

        set.IsActive = false;
        _setRepository.SaveChanges();
        
        game.GameStatus = EGameStatus.Ended;
        game.EndTime = DateTime.Now.ToUniversalTime();
        
        SetGameSetWins(game);
        _gameRepository.SaveChanges();
        
        _liveGameHubContext.EndGameAsync(gameId, game);
        return game;
    }

    public void DeleteGame(Guid gameId)
    {
        var game = _gameRepository.GetById(gameId);
        if (game == null) throw new Exception("Mängu ei leitud");
        if (game.GameStatus != EGameStatus.NotStarted) throw new Exception("Ainult alustamata mängu saab kustutada");

        var currentUserTeam = _teamService.GetMyTeam();
        if (game.HomeTeamId != currentUserTeam.Id) throw new Exception("Ainult enda loodud mänge saab kustutada");

        var gamePlayers = _playerInGameRepository.GetAllPlayersInGameByGameId(gameId);
        foreach (var gamePlayer in gamePlayers)
        {
            _playerInGameRepository.Delete(gamePlayer);
        }
        _playerInGameRepository.SaveChanges();
        
        _gameRepository.Delete(game);
        _gameRepository.SaveChanges();
    }

    public void CheckForMaxViews(Guid gameId, int currentViews)
    {
        var game = _gameRepository.GetById(gameId);
        if (game == null) return;

        if (game.MaxLiveViews < currentViews || game.MaxLiveViews == null)
        {
            game.MaxLiveViews = currentViews;
            _gameRepository.SaveChanges();
        }
    }

    private void SetGameSetWins(Game game)
    {
        var sets = _setRepository.GetCompletedSetsByGameId(game.Id);
        var homeTeamWins = 0;
        var awayTeamWins = 0;

        foreach (var set in sets)
        {
            if (set.HomeTeamScore > set.AwayTeamScore) homeTeamWins += 1;
            if (set.AwayTeamScore > set.HomeTeamScore) awayTeamWins += 1;
        }

        game.HomeTeamSetWins = homeTeamWins;
        game.AwayTeamSetWins = awayTeamWins;

    }
}
