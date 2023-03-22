using Interfaces.Base;

namespace Interfaces.Services;

public interface IHttpContextService : IBaseService
{
    Guid GetUserId();
    bool IsUserInRole(string role);
}