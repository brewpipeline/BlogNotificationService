namespace NotificationService.Interfaces.Bot;

public interface IBotClient
{
    Task<bool> SendNotifications(
        IEnumerable<long> userIds,
        string message,
        CancellationToken cancellation);
}