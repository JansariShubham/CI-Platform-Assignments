﻿using System;
using System.Collections.Generic;

namespace CIPlatform.entities.DataModels;

public partial class User
{
    public long UserId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    public string? Avatar { get; set; }

    public string? WhyIVolunteer { get; set; }

    public string? EmployeeId { get; set; }

    public string? Departmemt { get; set; }

    public short? CityId { get; set; }

    public byte? CountryId { get; set; }

    public string? ProfileText { get; set; }

    public string? LinkedInUrl { get; set; }

    public string? Title { get; set; }

    public byte Status { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset? UpdatedAt { get; set; }

    public DateTimeOffset? DeletedAt { get; set; }

    public byte? Avaibility { get; set; }

    public virtual City? City { get; set; }

    public virtual ICollection<Comment> Comments { get; } = new List<Comment>();

    public virtual ICollection<ContactUs> ContactUs { get; } = new List<ContactUs>();

    public virtual Country? Country { get; set; }

    public virtual ICollection<FavouriteMission> FavouriteMissions { get; } = new List<FavouriteMission>();

    public virtual LastCheck? LastCheck { get; set; }

    public virtual ICollection<MissionApplication> MissionApplications { get; } = new List<MissionApplication>();

    public virtual ICollection<MissionInvite> MissionInviteFromUsers { get; } = new List<MissionInvite>();

    public virtual ICollection<MissionInvite> MissionInviteToUsers { get; } = new List<MissionInvite>();

    public virtual ICollection<MissionRating> MissionRatings { get; } = new List<MissionRating>();

    public virtual NotificationSetting? NotificationSetting { get; set; }

    public virtual ICollection<Story> Stories { get; } = new List<Story>();

    public virtual ICollection<StoryInvite> StoryInviteFromUsers { get; } = new List<StoryInvite>();

    public virtual ICollection<StoryInvite> StoryInviteToUsers { get; } = new List<StoryInvite>();

    public virtual ICollection<Timesheet> Timesheets { get; } = new List<Timesheet>();

    public virtual ICollection<UserNotification> UserNotifications { get; } = new List<UserNotification>();

    public virtual ICollection<UserSkill> UserSkills { get; } = new List<UserSkill>();
}
