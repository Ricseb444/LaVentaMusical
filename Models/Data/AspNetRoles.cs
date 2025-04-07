using System;
using System.Collections.Generic;

namespace Models.Data;

public partial class AspNetRoles
{
    public string Id { get; set; } = null!;

    public string? Name { get; set; }

    public string? NormalizedName { get; set; }

    public string? ConcurrencyStamp { get; set; }

    public virtual ICollection<AspNetRoleClaims> AspNetRoleClaims { get; set; } = new List<AspNetRoleClaims>();

    public virtual ICollection<AspNetUsers> Users { get; set; } = new List<AspNetUsers>();
}
