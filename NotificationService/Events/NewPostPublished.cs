using System.Text.Json.Serialization;

namespace NotificationService.Events;

internal sealed class NewPostPublished
{
    [JsonPropertyName("blog_user_id")]
    public long BlogUserId { get; set; }

    [JsonPropertyName("post_sub_url")]
    public string PostSubUrl { get; set; } = string.Empty;
}