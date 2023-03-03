using System;
using System.Collections.Generic;

namespace CIPlatform.entities.DataModels;

public partial class Banner
{
    public long BannerId { get; set; }

    public byte[]? BannerImage { get; set; }

    public string? TextDesc { get; set; }

    public string? TextTitle { get; set; }

    public int SortOrder { get; set; }

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;

    public DateTimeOffset? UpdatedAt { get; set; }

    public DateTimeOffset? DeletedAt { get; set; }
}
