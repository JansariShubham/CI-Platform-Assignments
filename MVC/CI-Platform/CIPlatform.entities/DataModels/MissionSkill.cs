using System;
using System.Collections.Generic;

namespace CIPlatform.entities.DataModels;

public partial class MissionSkill
{
    public long MissionSkillId { get; set; }

    public int SkillId { get; set; }

    public long MissionId { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset? UpdatedAt { get; set; }

    public DateTimeOffset? DeletedAt { get; set; }

    public virtual Mission Mission { get; set; } = null!;

    public virtual Skill Skill { get; set; } = null!;
}
