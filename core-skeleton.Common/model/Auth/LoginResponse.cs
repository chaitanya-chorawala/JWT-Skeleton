namespace core_skeleton.Common.model.Auth;

public record LoginResponse
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}
