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
        NotificationServiceSettings getSettings(HostBuilderContext ctx) =>
            context.Configuration.GetSection(nameof(NotificationServiceSettings)).Get<NotificationServiceSettings>()
            ?? new();

        //TODO remove and register as transient when required
        var notificationSettings = getSettings(context);
        services.AddSingleton(notificationSettings);
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
