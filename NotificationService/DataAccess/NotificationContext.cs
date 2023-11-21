using Microsoft.EntityFrameworkCore;

namespace NotificationService.DataAccess;

internal class NotificationContext : DbContext
{
    public virtual DbSet<SubscriberDto> Subscribers { get; set; }

    public NotificationContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SubscriberDto>()
            .Property(e => e.LastUpdated)
            .IsRequired();

        base.OnModelCreating(modelBuilder);
    }
}