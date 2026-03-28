namespace Nucleos.Infrastructure.Gamification;

public interface IConquistaChecker
{
    Task CheckAndUnlock(Guid userId, string eventType, object data);
}
