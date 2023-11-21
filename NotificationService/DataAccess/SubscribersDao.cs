using Microsoft.EntityFrameworkCore;
using NotificationService.Domain;
using NotificationService.Interfaces;

namespace NotificationService.DataAccess;

internal class SubscribersDao(
    IDbContextFactory<NotificationContext> dbContext,
    ILogger<NotificationServiceWorker> logger) : ISubscribersDao
{
    //split into repository method if logic became more complicated
    public async Task Save(Subscriber subscriber, CancellationToken cancellation)
    {
        try
        {
            using var context = await dbContext.CreateDbContextAsync(cancellation);

            var existing = await context.Subscribers
                .Where(x => x.BlogUserId == subscriber.BlogUserId)
                .SingleOrDefaultAsync(cancellation);

            if (existing is null)
            {
                await context.AddAsync(subscriber.IntoDto(), cancellation);
                await context.SaveChangesAsync(cancellation);

                return;
            }

            if (existing.SendNotification != subscriber.SendNotification)
            {
                existing.SendNotification = subscriber.SendNotification;
                existing.LastUpdated = DateTime.UtcNow;

                await context.SaveChangesAsync(cancellation);
            }

            logger.LogInformation("There is nothing to update for {}", subscriber.BlogUserId);

        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while saving subscriber");
        }
    }
}