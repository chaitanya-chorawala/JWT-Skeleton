using core_skeleton.Common.ExceptionHandler;
using core_skeleton.Common.model;
using core_skeleton.Common.model.Auth;
using core_skeleton.Core.Contract.Common;
using core_skeleton.Core.Contract.Repository;
using core_skeleton.Core.Contract.Service;
using System.Reflection;

namespace core_skeleton.Persistence.Service;

public class AuthService : IAuthService
{
    private readonly ITokenGenerator _tokenGenerator;
    private readonly IUserRepo _userRepo;

    public AuthService(ITokenGenerator tokenGenerator, IUserRepo userRepo)
    {
        _tokenGenerator = tokenGenerator;
        _userRepo = userRepo;
    }

    public async Task<User> GetUser(Guid userid)
    {
        try
        {
            return await _userRepo.GetUserDetail(userid);            
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<LoginResponse> Login(Login model)
    {
        try
        {
            var user = await _userRepo.LoginAsync(model);
            return await _tokenGenerator.GenerateTokenAsync(user);
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<LoginResponse> RefreshToken(LoginResponse model)
    {
        try
        {           
            if (string.IsNullOrEmpty(model.AccessToken) || string.IsNullOrEmpty(model.RefreshToken))
                throw new BadRequestException("Please provide both tokens");

            return await _tokenGenerator.RegenerateTokenAsync(model);
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<RegisterResponse> Register(Register model)
    {
        try
        {
            return await _userRepo.RegisterAsync(model);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<IList<User>> UserList()
    {
        try
        {
            return await _userRepo.GetUserList();
        }
        catch (Exception)
        {
            throw;
        }
    }
}
