using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NotificationService.DataAccess;

[Table("subscribers", Schema = "notification")]
internal class SubscriberDto
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    public long BlogUserId { get; set; }

    public long TelegramId { get; set; }

    public bool SendNotification { get; set; }
}