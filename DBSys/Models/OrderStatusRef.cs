using System;
using System.Collections.Generic;

namespace DBSys.Models;

public partial class OrderStatusRef
{
    public string Status { get; set; } = null!;

    public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();
}
