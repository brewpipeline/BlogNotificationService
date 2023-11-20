using NotificationService;
using NotificationService.Configuration;

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, configBuilder) =>
    {
        configBuilder.SetBasePath(Directory.GetCurrentDirectory());
        configBuilder.AddJsonFile(Constants.ConfigFileName, optional: false);
    })
    .ConfigureServices((context, services) =>
    {
        NotificationServiceSettings getSettings(HostBuilderContext ctx) =>
            context.Configuration.GetSection(nameof(NotificationServiceSettings)).Get<NotificationServiceSettings>()
            ?? new();

        //TODO remove and register as transient when required
        var notificationSettings = getSettings(context);
        services.AddSingleton(notificationSettings);
        services.AddHostedService<NotificationServiceWorker>();

        services.AddRabbitMq(notificationSettings);
    });

var host = builder.Build();
await host.RunAsync();
