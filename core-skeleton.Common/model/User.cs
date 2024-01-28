namespace core_skeleton.Common.model;

public record User
{
    public Guid? Id { get; set; }
    public string? Username { get; set; }    
    public string? Email { get; set; }
    public string? Role { get; set; }
}
