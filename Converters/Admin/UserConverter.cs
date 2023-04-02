using AutoMapper;
using Domain.Identity;
using Interfaces.Converters;
using PublicAPI.DTO.Admin;

namespace Converters.Admin;

public class UsersConverter : Converter<AppUser, AppUserDto>, IUserConverter
{
    public UsersConverter(IMapper mapper) : base(mapper)
    {
    }

    public override AppUserDto Convert(AppUser entity)
    {
        return new AppUserDto()
        {
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            Email = entity.Email,
            IsActive = entity.IsActive,
            Name = entity.FirstName + " " + entity.LastName,
            Id = entity.Id
        };
    }
}