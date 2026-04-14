using System;
using System.Collections.Generic;

namespace DBSys.Models;

public partial class Sale
{
    public int SaleId { get; set; }

    public int? CustomerId { get; set; }

    public int? ProductId { get; set; }

    public int? VendorId { get; set; }

    public int? Quantity { get; set; }

    public decimal? UnitPriceAtSale { get; set; }

    public decimal? SubtotalAmount { get; set; }

    public decimal? TaxAmount { get; set; }

    public decimal? TotalAmount { get; set; }

    public string? Currency { get; set; }

    public string? Status { get; set; }

    public DateTime? PurchasedAt { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual Payment? Payment { get; set; }

    public virtual Product? Product { get; set; }

    public virtual OrderStatusRef? StatusNavigation { get; set; }

    public virtual Vendor? Vendor { get; set; }

    public virtual ICollection<RevenueReport> Reports { get; set; } = new List<RevenueReport>();
}
