using NotificationService.Domain;

namespace NotificationService.Interfaces;

public interface ISubscribersDao
{
    Task Save(Subscriber subscriber, CancellationToken cancellation);

    Task<IReadOnlyCollection<Subscriber>> GetActiveSubscribers(CancellationToken cancellation);
}