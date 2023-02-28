using System;
using System.Collections.Generic;

namespace CIPlatform.entities.DataModels;

public partial class MissionTheme
{
    public long MissionThemeId { get; set; }

    public string Title { get; set; } = null!;

    public byte Status { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset? UpdatedAt { get; set; }

    public DateTimeOffset? DeletedAt { get; set; }

    public virtual ICollection<Mission> Missions { get; } = new List<Mission>();
}
