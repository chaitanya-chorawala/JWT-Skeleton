namespace core_skeleton.Common.model.Auth;

public record Register
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}
