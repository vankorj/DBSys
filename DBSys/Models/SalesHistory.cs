using System;
using System.Collections.Generic;

namespace DBSys.Models;

public partial class SalesHistory
{
    public int HistoryId { get; set; }

    public int? SaleId { get; set; }

    public decimal? TotalAmount { get; set; }

    public string? Status { get; set; }

    public string? AuditAction { get; set; }

    public DateTime? AuditDate { get; set; }
}
