namespace NotificationService.Configuration;

internal sealed class NotificationServiceSettings
{
    public string MtRabbitMqConnectionString { get; set; } = string.Empty;
    public string QueueName { get; set; } = string.Empty;
}