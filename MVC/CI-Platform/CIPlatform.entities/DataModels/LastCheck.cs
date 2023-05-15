using System;
using System.Collections.Generic;

namespace CIPlatform.entities.DataModels;

public partial class LastCheck
{
    public long UserId { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual User User { get; set; } = null!;
}
