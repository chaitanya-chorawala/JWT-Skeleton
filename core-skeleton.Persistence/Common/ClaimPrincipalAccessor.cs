using core_skeleton.Common.model;
using core_skeleton.Core.Contract.Common;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace core_skeleton.Persistence.Common;

class ClaimPrincipalAccessor : IClaimPrincipalAccessor
{
    private readonly HttpContext? _httpContext;

    public ClaimPrincipalAccessor(IHttpContextAccessor httpContextAccessor)
    {
        _httpContext = httpContextAccessor.HttpContext;
    }
    public ClaimsPrincipal? ClaimsPrincipal => _httpContext?.User;

    public User User => new User()
    {
        Id = !string.IsNullOrEmpty(ClaimsPrincipal?.Claims?.Where(x => x.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value) ? new Guid(ClaimsPrincipal?.Claims?.Where(x => x.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value!) : null,
        Username = ClaimsPrincipal?.Claims?.Where(x => x.Type == ClaimTypes.Name).FirstOrDefault()?.Value,
        Email = ClaimsPrincipal?.Claims?.Where(x => x.Type == ClaimTypes.Email).FirstOrDefault()?.Value,
        Role = ClaimsPrincipal?.Claims?.Where(x => x.Type == ClaimTypes.Role).FirstOrDefault()?.Value,
    };
}
