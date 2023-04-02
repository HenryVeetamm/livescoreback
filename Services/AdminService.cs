using Domain.Identity;
using Exceptions;
using Interfaces.Services;
using Microsoft.AspNetCore.Identity;
using PublicAPI.DTO.Session;
using Services.Base;

namespace Services;

public class AdminService : Service, IAdminService
{
    private readonly IHttpContextService _httpContextService;
    private readonly RoleManager<AppRole> _roleManager;
    private readonly UserManager<AppUser> _userManager;

    public AdminService(UserManager<AppUser> userManager, IHttpContextService httpContextService, RoleManager<AppRole> roleManager)
    {
        _httpContextService = httpContextService;
        _roleManager = roleManager;
        _userManager = userManager;
    }
    
    public AppUser[] GetSystemUsers()
    {
        var users = _userManager.Users.ToArray();
        
        return users;
    }
    
    public async Task UpdatePassword(UpdatePasswordDto dto)
    {
        var userId = _httpContextService.GetUserId();
        if (userId == Guid.Empty) throw new Exception("Kasutajat ei leitud");
        var user = await _userManager.FindByIdAsync(userId.ToString());
        
        if (user == null) throw new Exception("Kasutajat ei leitud");
        
        var code = await _userManager.GeneratePasswordResetTokenAsync(user);
        
        await _userManager.ResetPasswordAsync(user, code, dto.Password);
    }

    public async Task RegisterUser(RegisterDto dto)
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
        if (!result.Succeeded) throw new LogicException("Registreerimine ebaõnnestus");
        
        var roleResult = _userManager.AddToRoleAsync(user, "user").Result;
        if (!roleResult.Succeeded) throw new LogicException("Rolli lisamine ebaõnnestus");
    }
}