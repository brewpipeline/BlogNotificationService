using System.Text;

namespace NotificationService.Domain;

public static class BlogMessage
{
    public static string GetNewPostMessage(string blogUrl, string subLink)
        => new StringBuilder()
            .AppendLine("Новая публикация на сайте")
            .AppendLine($"{blogUrl}{subLink}")
            .ToString();

    public static string GetSubscriptionMessage(bool isSubscribed)
        => isSubscribed
            ? "Вы подписались на уведомления"
            : "Вы отписались от уведомлений";
}