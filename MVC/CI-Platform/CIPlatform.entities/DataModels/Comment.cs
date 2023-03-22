using System;
using System.Collections.Generic;

namespace CIPlatform.entities.DataModels;

public partial class Comment
{
    public long CommentId { get; set; }

    public long? UserId { get; set; }

    public long? MissionId { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset? UpdatedAt { get; set; }

    public DateTimeOffset? DeletedAt { get; set; }

    public byte? ApprovalStatus { get; set; }

    public string? CommentMsg { get; set; }

    public virtual Mission? Mission { get; set; }

    public virtual User? User { get; set; }
}
