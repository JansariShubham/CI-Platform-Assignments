using System;
using System.Collections.Generic;

namespace CIPlatform.entities.DataModels;

public partial class Country
{
    public byte CountryId { get; set; }

    public string Name { get; set; } = null!;

    public string? Iso { get; set; }

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;
        
    public DateTimeOffset? UpdatedAt { get; set; }

    public DateTimeOffset? DeletedAt { get; set; }

    public virtual ICollection<City> Cities { get; } = new List<City>();

    public virtual ICollection<Mission> Missions { get; } = new List<Mission>();

    public virtual ICollection<User> Users { get; } = new List<User>();
}
