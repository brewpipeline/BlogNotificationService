namespace NotificationService;

public class NotificationServiceWorker(
    ILogger<NotificationServiceWorker> logger) : BackgroundService
{
    private readonly ILogger<NotificationServiceWorker> logger = logger;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (this.logger.IsEnabled(LogLevel.Information))
            {
                this.logger.LogInformation("NotificationServiceWorker running at: {time}", DateTimeOffset.Now);
            }
            await Task.Delay(50000, stoppingToken);
        }
    }
}
