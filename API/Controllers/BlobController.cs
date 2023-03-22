using Interfaces.Converters;
using Interfaces.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PublicAPI.DTO.Player;

namespace API.Controllers;

[Route("blobs")]
[ApiController]
public class BlobController : ControllerBase
{
    private readonly IBlobService _blobService;

    //Planned endpoints
    //1.Add player to team
    //2.Edit player in team
    //3.

    public BlobController(IBlobService blobService)
    {
        _blobService = blobService;
    }

    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult GetMyTeamPlayers()
    {
        var result = _blobService.GetBlobAsync("profilepictures", "testfile");
        
        return Ok(result);
    }

}