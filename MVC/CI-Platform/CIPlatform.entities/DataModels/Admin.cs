using System;
using System.Collections.Generic;

namespace CIPlatform.entities.DataModels;

public partial class Admin
{
    public byte AdminId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset? UpdatedAt { get; set; }

    public DateTimeOffset? DeletedAt { get; set; }

    public string? Avatar { get; set; }
}
