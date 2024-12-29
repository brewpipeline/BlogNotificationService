using Telegram.Bot;
using Telegram.Bot.Types;
namespace NotificationService.TelegramBot;

internal static class TelegramNotificationExtensions
{
    internal static async Task<(Chat? Chat, Exception? Exception)> TryGetChatByUserId(this Telegram.Bot.TelegramBotClient botClient, long userId)
    {
        try
        {
            var chat = await botClient.GetChatAsync(userId);
            return (chat, null);
        }
        catch (Exception ex)
        {
            return (null, ex);
        }
    }
}