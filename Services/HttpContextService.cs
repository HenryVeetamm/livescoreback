using System.Security.Claims;
using Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Services.Base;

namespace Services;

public class HttpContextService : Service, IHttpContextService
{
    private static IHttpContextAccessor _httpContextAccessor;
    private static ClaimsPrincipal User => _httpContextAccessor.HttpContext.User;

    public HttpContextService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    
    public Guid GetUserId()
    {
        
        var claim = User.FindFirst(ClaimTypes.NameIdentifier);

        return claim != null ? new Guid(claim.Value) : Guid.Empty;
    }

    public bool IsUserInRole(string role)
    {
        return User.IsInRole(role);
    }
}