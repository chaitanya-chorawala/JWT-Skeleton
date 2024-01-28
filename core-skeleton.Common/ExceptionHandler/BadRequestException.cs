namespace core_skeleton.Common.ExceptionHandler;

public class BadRequestException : Exception
{
    public BadRequestException(string? message) : base(message)
    {}
}
