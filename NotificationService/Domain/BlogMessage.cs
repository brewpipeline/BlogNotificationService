namespace NotificationService.Domain;

public static class BlogMessage
{
    public static string GetNewPostMessage(string blogUrl, string subLink)
        => $"Новая публикация {Environment.NewLine} {blogUrl}/{subLink}";
}