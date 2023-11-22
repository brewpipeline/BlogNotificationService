using NotificationService.Interfaces.Bot;

namespace NotificationService.TelegramBot;

internal class TelegramBotClient(IConfiguration configuration) : IBotClient
{
    private readonly string botToken = configuration.GetConnectionString(Constants.BotTokenConnectionString) ?? string.Empty;

    public async Task<bool> SendNotifications(IEnumerable<long> userIds, string message, CancellationToken cancellation)
    {
        foreach (var userId in userIds)
        {
            Console.WriteLine($"NOTIFICATION SENT {userId}- {message}");
        }

        return true;
    }
}