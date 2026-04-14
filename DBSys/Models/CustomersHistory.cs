using System;
using System.Collections.Generic;

namespace DBSys.Models;

public partial class CustomersHistory
{
    public int HistoryId { get; set; }

    public int? CustomerId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? AuditAction { get; set; }

    public DateTime? AuditDate { get; set; }
}
