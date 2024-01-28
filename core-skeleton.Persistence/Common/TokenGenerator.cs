using core_skeleton.Common.ExceptionHandler;
using core_skeleton.Common.model;
using core_skeleton.Common.model.Auth;
using core_skeleton.Core.Contract.Common;
using core_skeleton.Core.Contract.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace core_skeleton.Persistence.Common;

public class TokenGenerator : ITokenGenerator
{
    private readonly string _validAudience;
    private readonly string _validIssuer;
    private readonly string _secretKey;
    private readonly IUserRepo _userRepo;

    public TokenGenerator(IConfiguration configuration, IUserRepo userRepo)
    {
        _validAudience = configuration["JWT:ValidAudience"]!;
        _validIssuer = configuration["JWT:ValidIssuer"]!;
        _secretKey = configuration["JWT:Secret"]!;
        _userRepo = userRepo;
    }
    public async Task<LoginResponse> GenerateTokenAsync(User user)
    {
        try
        {
            LoginResponse response = new LoginResponse();
            var tokenHandler = new JwtSecurityTokenHandler();
            var secretBytes = Encoding.UTF8.GetBytes(_secretKey);
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(
                    new Claim[]
                    {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()!),
                    new Claim(ClaimTypes.Name, user.Username!),
                    new Claim(ClaimTypes.Email, user.Email!),
                    new Claim(ClaimTypes.Role, user.Role!)
                    }
                ),
                Audience = _validAudience,
                Issuer = _validIssuer,
                NotBefore = DateTime.Now,
                Expires = DateTime.Now.AddMinutes(15),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretBytes), SecurityAlgorithms.HmacSha256)
            };

            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var accessToken = tokenHandler.WriteToken(securityToken);

            string refreshToken = await GenerateRefreshTokenAsync(user.Id.ToString()!);
            response.AccessToken = accessToken;
            response.RefreshToken = refreshToken;

            return response;
        }
        catch (Exception)
        {
            throw;
        }        
    }

    public async Task<LoginResponse> RegenerateTokenAsync(LoginResponse model)
    {
        try
        {
            LoginResponse response = new LoginResponse();
            var expTokenHandler = new JwtSecurityTokenHandler();
            var expSecurityToken = (JwtSecurityToken)expTokenHandler.ReadToken(model.AccessToken);
            var username = expSecurityToken.Claims.FirstOrDefault(x => x.Type == "unique_name")?.Value;
            var userid = expSecurityToken.Claims.FirstOrDefault(x => x.Type == "nameid")?.Value;

            if (username is null || userid is null)
                throw new BadRequestException("Invalid access token");

            var existingRefreshToken = await _userRepo.VerifyRefreshTokenAsync(userid, model.RefreshToken);

            var tokenKey = Encoding.UTF8.GetBytes(_secretKey);
            var tokenHandler = new JwtSecurityToken(
                claims: expSecurityToken.Claims.ToArray(),
                audience: _validAudience,
                issuer: _validIssuer,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddMinutes(15),
                 signingCredentials: new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256)
                );

            var accessToken = new JwtSecurityTokenHandler().WriteToken(tokenHandler);
            string refreshToken = await GenerateRefreshTokenAsync(userid);

            response.AccessToken = accessToken;
            response.RefreshToken = refreshToken;
            return response;
        }
        catch (Exception)
        {
            throw;
        }
    }

    private async Task<string> GenerateRefreshTokenAsync(string userid)
    {
        try
        {
            var randomNumber = new Byte[32];
            using var randomNumberGenerator = RandomNumberGenerator.Create();
            randomNumberGenerator.GetBytes(randomNumber);
            string refreshToken = Convert.ToBase64String(randomNumber);

            //Save token to DB        
            await _userRepo.UpdateRefreshTokenAsync(userid, refreshToken);

            return refreshToken;
        }
        catch (Exception)
        {
            throw;
        }        
    }
}
