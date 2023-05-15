using System;
using System.Collections.Generic;

namespace CIPlatform.entities.DataModels;

public partial class UserNotification
{
    public long UserNotificationId { get; set; }

    public long? UserId { get; set; }

    public long? NotificationId { get; set; }

    public bool? IsRead { get; set; }

    public DateTimeOffset? CreatedAt { get; set; }

    public DateTimeOffset? UpdatedAt { get; set; }

    public virtual Notification? Notification { get; set; }

    public virtual User? User { get; set; }
}
