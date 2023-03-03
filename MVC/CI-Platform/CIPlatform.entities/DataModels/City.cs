using System;
using System.Collections.Generic;

namespace CIPlatform.entities.DataModels;

public partial class City
{
    public short CityId { get; set; }

    public byte? CountryId { get; set; }

    public string Name { get; set; } = null!;

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now; 

    public DateTimeOffset? UpdatedAt { get; set; }

    public DateTimeOffset? DeletedAt { get; set; }

    public virtual Country? Country { get; set; }

    public virtual ICollection<Mission> Missions { get; } = new List<Mission>();

    public virtual ICollection<User> Users { get; } = new List<User>();
}
