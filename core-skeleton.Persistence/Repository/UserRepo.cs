using core_skeleton.Common.ExceptionHandler;
using core_skeleton.Common.model;
using core_skeleton.Common.model.Auth;
using core_skeleton.Core.Contract.Repository;
using core_skeleton.Persistence.Configuration;
using Dapper;
using System.Data;
using System.Reflection;

namespace core_skeleton.Persistence.Repository;

public class UserRepo : IUserRepo
{
    private readonly ApplicationDbContext _context;

    public UserRepo(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<User> GetUserDetail(Guid userid)
    {
        try
        {
            using IDbConnection conn = _context.CreateConnection();
            var parameter = new
            {
                Userid = userid
            };

            var user = await conn.QueryFirstOrDefaultAsync<User>(
                "TestJwt_GetUser",
                param: parameter,
                commandType: CommandType.StoredProcedure);

            if (user == null)
                throw new NotFoundException("User not found");

            return user;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<IList<User>> GetUserList()
    {
        try
        {
            using IDbConnection conn = _context.CreateConnection();
            var users = await conn.QueryAsync<User>("TestJwt_UserList", commandType: CommandType.StoredProcedure);
            return users.ToList();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<User> LoginAsync(Login model)
    {
        try
        {
            using IDbConnection conn = _context.CreateConnection();
            var parameter = new
            {
                Username = model.Username,
                Password = model.Password
            };

            var user = await conn.QueryFirstOrDefaultAsync<User>(
                "TestJwt_LoginUser",
                param: parameter,
                commandType: CommandType.StoredProcedure);

            if (user == null)
                throw new NotFoundException("User not found");

            return user;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<RegisterResponse> RegisterAsync(Register model)
    {
        try
        {
            using IDbConnection conn = _context.CreateConnection();
            
            var message = await conn.QueryFirstAsync<RegisterResponse>(
                "TestJwt_RegisterUser",
                param: model,
                commandType: CommandType.StoredProcedure);            

            return message!;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task UpdateRefreshTokenAsync(string userid, string refreshToken)
    {
        try
        {
            using IDbConnection conn = _context.CreateConnection();
            var parameter = new
            {
                UserId = userid,
                Token = refreshToken
            };

            await conn.ExecuteAsync(
                "TestJwt_UpdateRefreshToken",
                param: parameter,
                commandType: CommandType.StoredProcedure);           
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<string> VerifyRefreshTokenAsync(string userid, string refreshToken)
    {
        try
        {
            using IDbConnection conn = _context.CreateConnection();
            var parameter = new
            {
                UserId = userid,
                Token = refreshToken
            };

            var token = await conn.QueryFirstOrDefaultAsync<string>(
                "TestJwt_VerifyRefreshToken",
                param: parameter,
                commandType: CommandType.StoredProcedure);

            if (token == null)
                throw new NotFoundException("User not found");
            return token;
        }
        catch (Exception)
        {
            throw;
        }
    }
}
