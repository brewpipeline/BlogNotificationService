namespace NotificationService.Domain;

public class Subscriber
{
    public long Id { get; init; }
    public long BlogUserId { get; init; }
    public long TelegramId { get; init; }
    public bool SendNotification { get; init; }
}