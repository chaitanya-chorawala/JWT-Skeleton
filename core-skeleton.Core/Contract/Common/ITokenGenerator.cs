using core_skeleton.Common.model;
using core_skeleton.Common.model.Auth;
using System.Security.Claims;

namespace core_skeleton.Core.Contract.Common;

public interface ITokenGenerator
{
    Task<LoginResponse> GenerateTokenAsync(User user);
    Task<LoginResponse> RegenerateTokenAsync(LoginResponse model);
}
