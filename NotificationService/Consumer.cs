using System.Text.Json;
using MassTransit;
using NotificationService.Events;

namespace NotificationService;

internal class Consumer : IConsumer<SubscriptionStateChanged>
{
    public async Task Consume(ConsumeContext<SubscriptionStateChanged> context)
    {
        var res = context.Message;
        var key = context.RoutingKey();

        Console.WriteLine($"Consumed {res.GetType()}: {JsonSerializer.Serialize(res)}");
    }
}