using Domain;
using Domain.Enums;
using Exceptions;
using Interfaces.Hubs;
using Interfaces.Repositories;
using Interfaces.Services;
using PublicAPI.DTO.PlayerInGame;
using Services.Base;

namespace Services;

public class PlayerInGameService: Service, IPlayerInGameService
{
    private readonly IPlayerInGameRepository _playerInGameRepository;
    private readonly ILiveGameHubContext _gameHubContext;

    public PlayerInGameService(
        IPlayerInGameRepository playerInGameRepository,
        ILiveGameHubContext gameHubContext)
    {
        _playerInGameRepository = playerInGameRepository;
        _gameHubContext = gameHubContext;
    }

    public PlayerInGame ManagePlayerResult(ManagePlayerPointsDto dto, Guid teamId, Guid gameId)
    {
        var playerInGame = _playerInGameRepository.GetWithPlayer(dto.PlayerInGameId);
        if (playerInGame == null) throw new LogicException("Not found");
        
        if (dto.Method == EMethod.Increment) Increment(playerInGame, dto);
        if (dto.Method == EMethod.Decrement) Decrement(playerInGame, dto);

        
        _playerInGameRepository.SaveChanges();
        _gameHubContext.PlayerDataChangedAsync(teamId, gameId, playerInGame, dto);
        return playerInGame;

    }

    private void Increment(PlayerInGame pig, ManagePlayerPointsDto dto)
    {
        switch (dto.Category)
        {
            case EPointCategory.Attack:
                switch (dto.CategoryResult)
                {
                    case ECategoryResult.Good:
                        pig.AttackToPoint += 1;
                        break;
                    case ECategoryResult.Neutral:
                        pig.AttackInGame += 1;
                        break;
                    case ECategoryResult.Bad:
                        pig.AttackFault += 1;
                        break;
                }
                break;

            case EPointCategory.Block:
                switch (dto.CategoryResult)
                {
                    case ECategoryResult.Good:
                        pig.BlockPoint += 1;
                        break;
                    case ECategoryResult.Bad:
                        pig.BlockFault += 1;
                        break;
                }
                break;
            case EPointCategory.Reception:
                switch (dto.CategoryResult)
                {
                    case ECategoryResult.Good:
                        pig.PerfectReception += 1;
                        break;
                    case ECategoryResult.Neutral:
                        pig.GoodReception += 1;
                        break;
                    case ECategoryResult.Bad:
                        pig.ReceptionFault += 1;
                        break;
                }
                break;
            case EPointCategory.Serve:
                switch (dto.CategoryResult)
                {
                    case ECategoryResult.Good:
                        pig.Aces += 1;
                        break;
                    case ECategoryResult.Bad:
                        pig.ServeFaults += 1;
                        break;
                }
                break;
        }
    }
    
    private void Decrement(PlayerInGame pig, ManagePlayerPointsDto dto)
    {
        switch (dto.Category)
        {
            case EPointCategory.Attack:
                switch (dto.CategoryResult)
                {
                    case ECategoryResult.Good:
                        pig.AttackToPoint -= 1;
                        break;
                    case ECategoryResult.Neutral:
                        pig.AttackInGame -= 1;
                        break;
                    case ECategoryResult.Bad:
                        pig.AttackFault -= 1;
                        break;
                }
                break;

            case EPointCategory.Block:
                switch (dto.CategoryResult)
                {
                    case ECategoryResult.Good:
                        pig.BlockPoint -= 1;
                        break;
                    case ECategoryResult.Bad:
                        pig.BlockFault -= 1;
                        break;
                }
                break;
            case EPointCategory.Reception:
                switch (dto.CategoryResult)
                {
                    case ECategoryResult.Good:
                        pig.PerfectReception -= 1;
                        break;
                    case ECategoryResult.Neutral:
                        pig.GoodReception -= 1;
                        break;
                    case ECategoryResult.Bad:
                        pig.ReceptionFault -= 1;
                        break;
                }
                break;
            case EPointCategory.Serve:
                switch (dto.CategoryResult)
                {
                    case ECategoryResult.Good:
                        pig.Aces -= 1;
                        break;
                    case ECategoryResult.Bad:
                        pig.ServeFaults -= 1;
                        break;
                }
                break;
        }
    }
}