using System.Text.Json.Serialization;

namespace NotificationService.Events;

internal sealed class SubscriptionStateChanged
{
    [JsonPropertyName("blog_user_id")]
    public long BlogUserId { get; set; }

    [JsonPropertyName("user_telegram_id")]
    public long UserTelegramId { get; set; }

    [JsonPropertyName("new_state")]
    public int NewState { get; set; }
}