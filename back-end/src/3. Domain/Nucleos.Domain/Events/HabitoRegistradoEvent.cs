namespace Nucleos.Domain.Events;
public record HabitoRegistradoEvent(Guid HabitoId, Guid UserId, DateTime OccurredAt);
