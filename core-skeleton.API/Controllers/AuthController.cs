using core_skeleton.Common.model.Auth;
using core_skeleton.Core.Contract.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace core_skeleton.API.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// Login to generate access and refresh token
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login(Login model)
    {
        var response = await _authService.Login(model);
        return Ok(response);
    }

    /// <summary>
    /// Register user
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register(Register model)
    {
        var response = await _authService.Register(model);
        return Ok(response);
    }

    /// <summary>
    /// Based on access and refresh token, API will return new access and refresh token
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken(LoginResponse model)
    {
        var response = await _authService.RefreshToken(model);
        return Ok(response);
    }

    /// <summary>
    /// Get all users after providing token
    /// </summary>
    /// <returns></returns>
    [HttpGet("user-list")]
    public async Task<IActionResult> GetUserList()
    {
        var response = await _authService.UserList();
        return Ok(response);
    }

    /// <summary>
    /// Get specific user after providing token
    /// </summary>
    /// <param name="userid"></param>
    /// <returns></returns>
    [HttpGet("user/{userid}")]
    public async Task<IActionResult> GetUserById(Guid userid)
    {
        var response = await _authService.GetUser(userid);
        return Ok(response);
    }
}
