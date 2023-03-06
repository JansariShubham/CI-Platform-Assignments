using System;
using System.Collections.Generic;

namespace CIPlatform.entities.DataModels;

public partial class GoalMission
{
    public long GoalMissionId { get; set; }

    public long? MissionId { get; set; }

    public string? GoalObjectiveText { get; set; }

    public int GoalValue { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset? UpdatedAt { get; set; }

    public DateTimeOffset? DeletedAt { get; set; }

    public virtual Mission? Mission { get; set; }
}
