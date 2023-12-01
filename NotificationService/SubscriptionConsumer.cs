using MassTransit;
using NotificationService.Domain;
using NotificationService.Events;
using NotificationService.Interfaces;
using NotificationService.Interfaces.Bot;

namespace NotificationService;

internal class SubscriptionConsumer(
    ISubscribersDao subscribersDao,
    IBotClient botClient,
    ILogger<SubscriptionConsumer> logger)
    : IConsumer<SubscriptionStateChanged>,
    IConsumeMessageByHeader
{
    public string HeaderValue => "subscriptionstatechanged";

    public async Task Consume(ConsumeContext<SubscriptionStateChanged> context)
    {
        var stateChanged = context.Message;

        var newSubscriptionState = stateChanged.NewState == 1;
        var subscriber = new Subscriber
        {
            Id = 0,
            BlogUserId = stateChanged.BlogUserId,
            TelegramId = stateChanged.UserTelegramId,
            SendNotification = newSubscriptionState,
        };
        await subscribersDao.Save(subscriber, CancellationToken.None);

        await botClient.SendNotifications(
            new List<long> { subscriber.TelegramId },
            BlogMessage.GetSubscriptionMessage(newSubscriptionState),
            CancellationToken.None);

        logger.LogInformation("Event consumed. External user Id {}", subscriber.BlogUserId);
    }
}