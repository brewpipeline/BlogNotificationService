using MassTransit;
using NotificationService.Domain;
using NotificationService.Events;
using NotificationService.Interfaces;

namespace NotificationService;

internal class SubscriptionConsumer(ISubscribersDao subscribersDao, ILogger<NotificationServiceWorker> logger) : IConsumer<SubscriptionStateChanged>
{
    public async Task Consume(ConsumeContext<SubscriptionStateChanged> context)
    {
        var stateChanged = context.Message;

        var subscriber = new Subscriber
        {
            Id = 0,
            BlogUserId = stateChanged.BlogUserId,
            TelegramId = stateChanged.UserTelegramId,
            SendNotification = stateChanged.NewState == 1,
        };
        await subscribersDao.Save(subscriber, CancellationToken.None);

        logger.LogInformation("Event consumed. External user Id {}", subscriber.BlogUserId);
    }
}