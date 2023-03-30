using System;
using System.Collections.Generic;

namespace CIPlatform.entities.DataModels;

public partial class Story
{
    public int StoryId { get; set; }

    public long? UserId { get; set; }

    public long? MissionId { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public byte Status { get; set; }

    public DateTimeOffset? PublishedAt { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset? UpdatedAt { get; set; }

    public DateTimeOffset? DeletedAt { get; set; }

    public long? StoryViews { get; set; }

    public virtual Mission? Mission { get; set; }

    public virtual ICollection<StoryInvite> StoryInvites { get; } = new List<StoryInvite>();

    public virtual ICollection<StoryMedia> StoryMedia { get; } = new List<StoryMedia>();

    public virtual User? User { get; set; }
}
