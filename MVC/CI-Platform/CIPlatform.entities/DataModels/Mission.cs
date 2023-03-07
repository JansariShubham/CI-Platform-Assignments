using System;
using System.Collections.Generic;

namespace CIPlatform.entities.DataModels;

public partial class Mission
{
    public long MissionId { get; set; }

    public long? ThemeId { get; set; }

    public short? CityId { get; set; }

    public byte? CountryId { get; set; }

    public string Title { get; set; } = null!;

    public string ShortDesc { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public bool MissionType { get; set; }

    public bool Status { get; set; }

    public string? OrgName { get; set; }

    public string? OrgDetails { get; set; }

    public byte? Availability { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset? UpdatedAt { get; set; }

    public DateTimeOffset? DeletedAt { get; set; }

    public virtual City? City { get; set; }

    public virtual ICollection<Comment> Comments { get; } = new List<Comment>();

    public virtual Country? Country { get; set; }

    public virtual ICollection<FavouriteMission> FavouriteMissions { get; } = new List<FavouriteMission>();

    public virtual ICollection<GoalMission> GoalMissions { get; } = new List<GoalMission>();

    public virtual ICollection<MissionApplication> MissionApplications { get; } = new List<MissionApplication>();

    public virtual ICollection<MissionDocument> MissionDocuments { get; } = new List<MissionDocument>();

    public virtual ICollection<MissionInvite> MissionInvites { get; } = new List<MissionInvite>();

    public virtual ICollection<MissionMedia> MissionMedia { get; } = new List<MissionMedia>();

    public virtual ICollection<MissionRating> MissionRatings { get; } = new List<MissionRating>();

    public virtual ICollection<MissionSkill> MissionSkills { get; } = new List<MissionSkill>();

    public virtual MissionTheme? Theme { get; set; }
}
