using NotificationService.Interfaces.Bot;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace NotificationService.TelegramBot;

internal class TelegramBotClient : IBotClient
{
    private readonly Telegram.Bot.TelegramBotClient botClient;
    private readonly ILogger<TelegramBotClient> logger;

    //TODO do not send if telegram init failed
    public TelegramBotClient(IConfiguration configuration, ILogger<TelegramBotClient> logger)
    {
        var botToken = configuration.GetConnectionString(Constants.BotTokenConnectionString) ?? string.Empty;
        botClient = new Telegram.Bot.TelegramBotClient(botToken);
        this.logger = logger;
    }

    public async Task<bool> SendNotifications(IEnumerable<long> userIds, string message, CancellationToken cancellation)
    {
        if (!userIds.Any())
            return false;

        foreach (var userId in userIds)
        {
            using var scope = logger.BeginScope("UserId: {}", userId);

            var result = await botClient.TryGetChatByUserId(userId);
            if (result.Chat is null)
            {
                logger.LogError(result.Exception, "Could not find chat by userID");
                continue;
            }

            try
            {
                await botClient.SendTextMessageAsync(result.Chat.Id, message, cancellationToken: cancellation);
                logger.LogTrace("NOTIFICATIONs SENT - {}", message);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "error sending message");
            }
        }
        return true;
    }
}