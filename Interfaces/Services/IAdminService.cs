using Domain.Identity;
using Interfaces.Base;
using PublicAPI.DTO.Session;

namespace Interfaces.Services;

public interface IAdminService : IBaseService
{
    AppUser[] GetSystemUsers();
    Task UpdatePassword(UpdatePasswordDto dto);
    Task RegisterUser(RegisterDto dto);
}