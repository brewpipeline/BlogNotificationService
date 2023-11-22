using Microsoft.EntityFrameworkCore;
using NotificationService;
using NotificationService.Configuration;
using NotificationService.DataAccess;

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, configBuilder) =>
    {
        configBuilder.SetBasePath(Directory.GetCurrentDirectory());
        configBuilder.AddJsonFile(Constants.ConfigFileName, optional: false);
    })
    .ConfigureServices((context, services) =>
    {
        //rly??!!?
        NotificationServiceSettings getSettings(HostBuilderContext ctx) =>
            context.Configuration.GetSection(nameof(NotificationServiceSettings)).Get<NotificationServiceSettings>()
            ?? new();
        var sendingSettings = context.Configuration
            .GetSection(nameof(SendingSettings))
            .Get<SendingSettings>() ?? new();

        if (!sendingSettings.IsValid())
        {
            throw new ArgumentException("Send settings should be set"); // do not throw
        }

        //TODO remove and register as transient when required
        var notificationSettings = getSettings(context);
        services.AddSingleton(notificationSettings);
        services.AddSingleton(sendingSettings);

        services.AddHostedService<NotificationServiceWorker>();

        services.AddDomain();
        services.AddRabbitMq(notificationSettings);

        services.AddDbContextFactory<NotificationContext>((services, optionsBuilder) =>
        {
            var connectionString = context.Configuration.GetConnectionString(Constants.PostgresConnectionString);
            optionsBuilder.UseNpgsql(connectionString);
        });

    });

var host = builder.Build();

using (var scope = host.Services.CreateScope())
{
    var ctx = scope.ServiceProvider.GetRequiredService<NotificationContext>();
    await ctx.Database.MigrateAsync();
}

await host.RunAsync();
