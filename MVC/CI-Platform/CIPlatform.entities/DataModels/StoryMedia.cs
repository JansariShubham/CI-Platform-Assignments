using System;
using System.Collections.Generic;

namespace CIPlatform.entities.DataModels;

public partial class StoryMedia
{
    public int StoryMediaId { get; set; }

    public int? StoryId { get; set; }

    public string MediaName { get; set; } = null!;

    public string MadiaPath { get; set; } = null!;

    public string MediaType { get; set; } = null!;

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset? UpdatedAt { get; set; }

    public DateTimeOffset? DeletedAt { get; set; }

    public virtual Story? Story { get; set; }
}
