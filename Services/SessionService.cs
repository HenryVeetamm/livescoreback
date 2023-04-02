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
    private readonly RoleManager<AppRole> _roleManager;

    public SessionService(
        UserManager<AppUser> userManager, 
        SignInManager<AppUser> signInManager,
        IConfiguration configuration,
        ITeamRepository teamRepository,
        RoleManager<AppRole> roleManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
        _teamRepository = teamRepository;
        _roleManager = roleManager;
    }

    public async Task<AuthorizationDto> Register(RegisterDto dto)
    {
        var existingUser = await _userManager.FindByEmailAsync(dto.Email);
        if (existingUser != null) throw new LogicException("Kasutaja on juba registreeritud.");

        var user = new AppUser
        {
            Email = dto.Email,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            UserName = dto.Email,
            IsActive = true
        };

        var result = await _userManager.CreateAsync(user, dto.Password);
        var roleResult = _userManager.AddToRoleAsync(user, "admin").Result;
        if (result.Succeeded) return new AuthorizationDto { Token = "OK" };

        var token = GenerateJwtToken(user, "admin");
        
        return new AuthorizationDto { Token = token };

    }

    //Should add team-id
    public async Task<AuthorizationDto> Login(LoginDto dto)
    {
        var appUser = await _userManager.FindByEmailAsync(dto.Email);
        if (appUser == null) throw new LogicException("Kasutajat ei leitud");
        if (!appUser.IsActive) throw new LogicException("Kasutaja ei ole aktiivne");
        
        var result = await _signInManager.CheckPasswordSignInAsync(appUser, dto.Password, false);
        if (!result.Succeeded) throw new LogicException("Kasutaja või parool on vale.");
        var roles = (await _userManager.GetRolesAsync(appUser)).FirstOrDefault();

        var token = GenerateJwtToken(appUser, roles);
        return new AuthorizationDto { Token = token };
    }

    private string GenerateJwtToken(AppUser appUser, string? role)
    {
        var team = _teamRepository.GetByUserId(appUser.Id);
        var teamId = team != null ? team.Id.ToString() : "";
        
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, appUser.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.GivenName, appUser.FirstName),
            new Claim(JwtRegisteredClaimNames.FamilyName, appUser.LastName),
            new Claim("role", role ?? "user"),
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