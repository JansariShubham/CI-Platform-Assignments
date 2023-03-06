using System;
using System.Collections.Generic;

namespace CIPlatform.entities.DataModels;

public partial class MissionMedium
{
    public long MissionMediaId { get; set; }

    public long? MissionId { get; set; }

    public string MediaName { get; set; } = null!;

    public string MediaType { get; set; } = null!;

    public string? MediaPath { get; set; }

    public bool? DefaultMedia { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset? UpdatedAt { get; set; }

    public DateTimeOffset? DeletedAt { get; set; }

    public virtual Mission? Mission { get; set; }
}
