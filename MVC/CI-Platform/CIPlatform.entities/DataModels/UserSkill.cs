using System;
using System.Collections.Generic;

namespace CIPlatform.entities.DataModels;

public partial class UserSkill
{
    public long UserSkillId { get; set; }

    public long? UserId { get; set; }

    public int? SkillId { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset? UpdatedAt { get; set; }

    public DateTimeOffset? DeletedAt { get; set; }

    public virtual Skill? Skill { get; set; }

    public virtual User? User { get; set; }
}
