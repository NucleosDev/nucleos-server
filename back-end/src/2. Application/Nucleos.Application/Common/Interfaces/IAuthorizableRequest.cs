public interface IAuthorizableRequest
{
    string? Permission { get; }
    Guid? UserId { get; }
}