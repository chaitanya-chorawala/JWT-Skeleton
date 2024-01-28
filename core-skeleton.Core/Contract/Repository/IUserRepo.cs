using core_skeleton.Common.model;
using core_skeleton.Common.model.Auth;

namespace core_skeleton.Core.Contract.Repository;

public interface IUserRepo
{
    Task<RegisterResponse> RegisterAsync(Register model);
    Task<User> LoginAsync(Login model);
    Task<string> VerifyRefreshTokenAsync(string userid, string refreshToken);
    Task UpdateRefreshTokenAsync(string userid, string refreshToken);
    Task<IList<User>> GetUserList();
    Task<User> GetUserDetail(Guid userid);
}
