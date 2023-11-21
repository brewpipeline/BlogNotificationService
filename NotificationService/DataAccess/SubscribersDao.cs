using Microsoft.EntityFrameworkCore;
using NotificationService.Domain;
using NotificationService.Interfaces;

namespace NotificationService.DataAccess;

internal class SubscribersDao(IDbContextFactory<NotificationContext> dbContext) : ISubscribersDao
{
    //split into repository method if logic became more complicated
    public async Task Save(Subscriber subscriber, CancellationToken cancellation)
    {
        Console.WriteLine("Saved XD");
    }
}