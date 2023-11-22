namespace NotificationService.Configuration;

public class SendingSettings
{
    public string SiteUrl { get; set; } = string.Empty;

    public bool IsValid() => !string.IsNullOrWhiteSpace(SiteUrl);
}

