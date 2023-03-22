using Interfaces.Base;
using PublicAPI.DTO.Session;

namespace Interfaces.Services;

public interface ISessionService : IBaseService
{
    Task<AuthorizationDto> Register(RegisterDto dto);
    Task<AuthorizationDto> Login(LoginDto dto);
}