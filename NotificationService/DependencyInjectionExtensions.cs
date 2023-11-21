using MassTransit;
using NotificationService.DataAccess;
using NotificationService.Events;
using NotificationService.Interfaces;

namespace NotificationService;

internal static class DependencyInjectionExtensions
{
    private const int HOST_HEARTBEAT_SECONDS = 300;


    internal static IServiceCollection AddRabbitMq(this IServiceCollection services, Configuration.NotificationServiceSettings notificationServiceSettings)
    {

        services.AddTransient<IConsumer<SubscriptionStateChanged>, SubscriptionConsumer>();
        services.AddMassTransit(s =>
        {
            s.AddConsumer<SubscriptionConsumer>();

            s.UsingRabbitMq((context, configure) =>
            {
                configure.ClearSerialization();
                configure.UseRawJsonSerializer();
                configure.UseRawJsonDeserializer(RawSerializerOptions.AnyMessageType);
                configure.Host(
                    new Uri(notificationServiceSettings.MtRabbitMqConnectionString),
                    $"NotificationServiceReceiver.{notificationServiceSettings.QueueName}",
                    c => c.Heartbeat(HOST_HEARTBEAT_SECONDS)
                );
                configure.ReceiveEndpoint(notificationServiceSettings.QueueName, x =>
                {
                    x.Bind(notificationServiceSettings.QueueName);
                    x.ConcurrentMessageLimit = 1;
                    x.ConfigureConsumeTopology = false;

                    x.ConfigureConsumer<SubscriptionConsumer>(context);
                });
            });

        });

        return services;
    }

    internal static IServiceCollection AddDomain(this IServiceCollection services)
        => services.AddTransient<ISubscribersDao, SubscribersDao>();

}

