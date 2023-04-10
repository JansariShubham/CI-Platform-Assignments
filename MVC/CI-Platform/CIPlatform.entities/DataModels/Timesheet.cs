using System;
using System.Collections.Generic;

namespace CIPlatform.entities.DataModels;

public partial class Timesheet
{
    public long TimesheetId { get; set; }

    public long? UserId { get; set; }

    public long? MissionId { get; set; }

    public int? Action { get; set; }

    public DateTimeOffset DateVolunteered { get; set; }

    public string? Notes { get; set; }

    public bool? Status { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset? UpdatedAt { get; set; }

    public DateTimeOffset? DeletedAt { get; set; }

    public int? Hour { get; set; }

    public int? Minutes { get; set; }

    public virtual Mission? Mission { get; set; }

    public virtual User? User { get; set; }
}
