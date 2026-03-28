namespace Nucleos.Domain.Events;
public record UserRegisteredEvent(Guid UserId, string Email, DateTime OccurredAt);
