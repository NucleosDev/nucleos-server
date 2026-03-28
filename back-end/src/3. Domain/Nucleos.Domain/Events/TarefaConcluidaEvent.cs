namespace Nucleos.Domain.Events;
public record TarefaConcluidaEvent(Guid TarefaId, Guid UserId, DateTime OccurredAt);
