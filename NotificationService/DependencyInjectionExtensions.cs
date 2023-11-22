using MassTransit;
using NotificationService.DataAccess;
using NotificationService.Interfaces;
using NotificationService.Interfaces.Bot;
using NotificationService.TelegramBot;

namespace NotificationService;

internal static class DependencyInjectionExtensions
{
    private const int HOST_HEARTBEAT_SECONDS = 300;

    internal static IServiceCollection AddRabbitMq(this IServiceCollection services, Configuration.NotificationServiceSettings notificationServiceSettings)
    {
        services.AddMassTransit(s =>
        {
            s.AddConsumer<SubscriptionConsumer>();
            s.AddConsumer<PostConsumer>();

            s.UsingRabbitMq((context, configure) =>
            {
                configure.UseRawJsonDeserializer(RawSerializerOptions.CopyHeaders, isDefault: true);
                configure.Host(
                    new Uri(notificationServiceSettings.MtRabbitMqConnectionString),
                    $"NotificationServiceReceiver.{notificationServiceSettings.QueueName}",
                    c => c.Heartbeat(HOST_HEARTBEAT_SECONDS)
                );
                configure.ReceiveEndpoint(notificationServiceSettings.QueueName, x =>
                {
                    x.Bind(notificationServiceSettings.QueueName);
                    x.ConcurrentMessageLimit = 1;
                    x.ConfigureConsumeTopology = true;

                    x.ConfigureConsumerWithHeaderRouting<SubscriptionConsumer>(context);
                    x.ConfigureConsumerWithHeaderRouting<PostConsumer>(context);

                });
            });

        });

        return services;
    }

    internal static IReceiveEndpointConfigurator ConfigureConsumerWithHeaderRouting<T>(
        this IReceiveEndpointConfigurator configurator,
        IBusRegistrationContext context) where T : class, IConsumeMessageByHeader, IConsumer
    {
        configurator.ConfigureConsumer<T>(
            context,
            x => x.UseContextFilter(
                v => Task.FromResult(
                    v.GetHeader<string>(v.Consumer.HeaderKey) == v.Consumer.HeaderValue)));
        return configurator;
    }

    internal static IServiceCollection AddDomain(this IServiceCollection services)
        => services
            .AddTransient<ISubscribersDao, SubscribersDao>()
            .AddSingleton<IBotClient, TelegramBotClient>();

}

