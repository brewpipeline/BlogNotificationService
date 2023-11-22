namespace NotificationService.Domain;

public static class BlogMessage
{
    public static string GetNewPostMessage(string blogUrl, string subLink)
        => $"{blogUrl}{subLink}";
}