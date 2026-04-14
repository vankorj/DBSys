using System;
using System.Collections.Generic;

namespace DBSys.Models;

public partial class ProductsHistory
{
    public int HistoryId { get; set; }

    public int? ProductId { get; set; }

    public string? Name { get; set; }

    public decimal? UnitPrice { get; set; }

    public int? InventoryQty { get; set; }

    public string? AuditAction { get; set; }

    public DateTime? AuditDate { get; set; }
}
