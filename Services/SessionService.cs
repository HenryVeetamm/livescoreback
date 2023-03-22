using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain.Identity;
using Exceptions;
using Interfaces.Repositories;
using Interfaces.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PublicAPI.DTO.Session;
using Services.Base;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Services;

public class SessionService: Service, ISessionService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IConfiguration _configuration;
    private readonly ITeamRepository _teamRepository;

    public SessionService(
        UserManager<AppUser> userManager, 
        SignInManager<AppUser> signInManager,
        IConfiguration configuration,
        ITeamRepository teamRepository)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
        _teamRepository = teamRepository;
    }

    public async Task<AuthorizationDto> Register(RegisterDto dto)
    {
        var existingUser = await _userManager.FindByEmailAsync(dto.Email);
        if (existingUser != null) throw new ArgumentException("user registered");

        var user = new AppUser
        {
            Email = dto.Email,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            UserName = dto.Email
        };

        var result = await _userManager.CreateAsync(user, dto.Password);
        if (result.Succeeded) return new AuthorizationDto { Token = "OK" };
        
        var token = GenerateJwtToken(user);
        
        return new AuthorizationDto { Token = token };

    }

    //Should add team-id
    public async Task<AuthorizationDto> Login(LoginDto dto)
    {
        var appUser = await _userManager.FindByEmailAsync(dto.Email);
        if (appUser == null) throw new LogicException("Not found");

        var result = await _signInManager.CheckPasswordSignInAsync(appUser, dto.Password, false);
        if (!result.Succeeded) throw new LogicException("Kasutaja või parool on vale.");

        var token = GenerateJwtToken(appUser);
        return new AuthorizationDto { Token = token };
    }

    private string GenerateJwtToken(AppUser appUser)
    {
        //TEAMID nuputada.
        var team = _teamRepository.GetByUserId(appUser.Id);
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, appUser.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.GivenName, appUser.FirstName),
            new Claim(JwtRegisteredClaimNames.FamilyName, appUser.LastName),
            new Claim("role", "admin"),
            new Claim("team_id", team.Id.ToString())
        };
        var signingKey =  new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
        var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            _configuration["JWT:Issuer"],
            _configuration["JWT:Issuer"],
            claims,
            expires: DateTime.Now.AddMinutes(short.Parse(_configuration["JWT:ExpireInMinutes"])),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}