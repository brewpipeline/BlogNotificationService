using NotificationService.Domain;

namespace NotificationService.DataAccess;

internal static class DtoExtensions
{
    public static SubscriberDto IntoDto(this Subscriber subscriber)
        => new()
        {
            BlogUserId = subscriber.BlogUserId,
            SendNotification = subscriber.SendNotification,
            TelegramId = subscriber.TelegramId,
            LastUpdated = DateTime.UtcNow,
        };

    public static Subscriber IntoDomain(this SubscriberDto subscriberDto)
        => new()
        {
            Id = subscriberDto.Id,
            BlogUserId = subscriberDto.BlogUserId,
            TelegramId = subscriberDto.TelegramId,
            SendNotification = subscriberDto.SendNotification,
        };
}