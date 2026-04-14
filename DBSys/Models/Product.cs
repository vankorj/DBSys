using System;
using System.Collections.Generic;

namespace DBSys.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public int VendorId { get; set; }

    public string? Sku { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public decimal? UnitPrice { get; set; }

    public string? Currency { get; set; }

    public int? InventoryQty { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();

    public virtual Vendor Vendor { get; set; } = null!;
}
