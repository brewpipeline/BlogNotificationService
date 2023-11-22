using MassTransit;
using NotificationService.Configuration;
using NotificationService.Domain;
using NotificationService.Events;
using NotificationService.Interfaces;
using NotificationService.Interfaces.Bot;

namespace NotificationService;

internal class PostConsumer(
    ISubscribersDao subscribersDao,
    IBotClient botClient,
    SendingSettings settings,
    ILogger<PostConsumer> logger)
    : IConsumer<NewPostPublished>, IConsumeMessageByHeader
{
    public string HeaderValue => "newpostpublished";

    public async Task Consume(ConsumeContext<NewPostPublished> context)
    {
        var postPublished = context.Message;

        var usersToNotify = await subscribersDao.GetActiveSubscribers(CancellationToken.None);

        await botClient.SendNotifications(
            usersToNotify.Select(x => x.TelegramId).ToList(),
            BlogMessage.GetNewPostMessage(settings.SiteUrl, postPublished.PostSubUrl),
            CancellationToken.None);

        logger.LogInformation("Event consumed. Post with suburl {}", postPublished.PostSubUrl);
    }
}
