using System;
using System.Collections.Generic;

namespace CIPlatform.entities.DataModels;

public partial class MissionRating
{
    public long MissionRatingId { get; set; }

    public long UserId { get; set; }

    public long MissionId { get; set; }

    public byte Rating { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset? UpdatedAt { get; set; }

    public DateTimeOffset? DeletedAt { get; set; }

    public virtual Mission Mission { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
