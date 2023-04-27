using System;
using System.Collections.Generic;

namespace CIPlatform.entities.DataModels;

public partial class Banner
{
    public long BannerId { get; set; }

    public string? BannerImage { get; set; }

    public string? TextDesc { get; set; }

    public string? TextTitle { get; set; }

    public int SortOrder { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset? UpdatedAt { get; set; }

    public DateTimeOffset? DeletedAt { get; set; }

    public bool? Status { get; set; }
}
