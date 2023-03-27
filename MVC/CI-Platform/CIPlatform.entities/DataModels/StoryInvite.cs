using System;
using System.Collections.Generic;

namespace CIPlatform.entities.DataModels;

public partial class StoryInvite
{
    public int StoryInviteId { get; set; }

    public int? StoryId { get; set; }

    public long? FromUserId { get; set; }

    public long? ToUserId { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset? UpdatedAt { get; set; }

    public DateTimeOffset? DeletedAt { get; set; }

    public virtual User? FromUser { get; set; }

    public virtual Story? Story { get; set; }

    public virtual User? ToUser { get; set; }
}
