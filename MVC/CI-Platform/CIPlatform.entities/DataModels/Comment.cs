using System;
using System.Collections.Generic;

namespace CIPlatform.entities.DataModels;

public partial class Comment
{
    public long CommentId { get; set; }

    public long? UserId { get; set; }

    public long? MissionId { get; set; }

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;

    public DateTimeOffset? UpdatedAt { get; set; }

    public DateTimeOffset? DeletedAt { get; set; }

    public virtual Mission? Mission { get; set; }

    public virtual User? User { get; set; }
}
