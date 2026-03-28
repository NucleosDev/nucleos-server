namespace Nucleos.Domain.Events;
public record NivelAlcancadoEvent(Guid UserId, int NovoNivel, DateTime OccurredAt);
