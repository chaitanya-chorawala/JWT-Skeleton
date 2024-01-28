using core_skeleton.Common.model;
using System.Security.Claims;

namespace core_skeleton.Core.Contract.Common;

public interface IClaimPrincipalAccessor
{
    ClaimsPrincipal? ClaimsPrincipal { get; }
    User User { get; }
}
