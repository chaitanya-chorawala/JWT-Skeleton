using core_skeleton.Common.model;
using core_skeleton.Common.model.Auth;

namespace core_skeleton.Core.Contract.Service;

public interface IAuthService
{
    Task<LoginResponse> Login(Login model);
    Task<RegisterResponse> Register(Register model);
    Task<LoginResponse> RefreshToken(LoginResponse model);
    Task<IList<User>> UserList(); 
    Task<User> GetUser(Guid userid); 
}
