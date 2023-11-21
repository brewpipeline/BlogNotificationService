using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NotificationService.DataAccess;

[Table("subscribers", Schema = "notification")]
[Index(nameof(BlogUserId), IsUnique = true)]
[Index(nameof(TelegramId), IsUnique = true)]
internal class SubscriberDto
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    public long BlogUserId { get; set; }

    public long TelegramId { get; set; }

    public bool SendNotification { get; set; }

    public DateTime LastUpdated { get; set; }
}