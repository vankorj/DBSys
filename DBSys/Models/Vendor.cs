using System;
using System.Collections.Generic;

namespace DBSys.Models;

public partial class Vendor
{
    public int VendorId { get; set; }

    public string? Name { get; set; }

    public string? ContactEmail { get; set; }

    public string? PayoutAccountRef { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();
}
