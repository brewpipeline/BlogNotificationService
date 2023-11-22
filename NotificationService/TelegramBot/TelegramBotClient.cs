using NotificationService.Interfaces.Bot;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace NotificationService.TelegramBot;

internal class TelegramBotClient : IBotClient
{
    private readonly Telegram.Bot.TelegramBotClient botClient;

    //TODO do not send if telegram init failed
    public TelegramBotClient(IConfiguration configuration)
    {
        var botToken = configuration.GetConnectionString(Constants.BotTokenConnectionString) ?? string.Empty;
        botClient = new Telegram.Bot.TelegramBotClient(botToken);
    }

    public async Task<bool> SendNotifications(IEnumerable<long> userIds, string message, CancellationToken cancellation)
    {
        if (!userIds.Any())
            return false;

        foreach (var userId in userIds)
        {
            var chat = await GetTelegramChatByUserId(userId);

            if (chat is null)
            {
                Console.WriteLine("Could not find chat by userID");
                continue;
            }

            await botClient.SendTextMessageAsync(chat.Id, message, cancellationToken: cancellation);
        }

        Console.WriteLine($"NOTIFICATIONs SENT - {message}");

        return true;
    }

    private async Task<Chat?> GetTelegramChatByUserId(long telegramUserId)
        => await botClient.GetChatAsync(telegramUserId);
}