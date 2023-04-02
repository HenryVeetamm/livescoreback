using Domain.Identity;
using PublicAPI.DTO.Admin;

namespace Interfaces.Converters;

public interface IUserConverter : IConverter<AppUser, AppUserDto>
{
    
}