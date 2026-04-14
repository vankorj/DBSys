using System;
using System.Collections.Generic;

namespace DBSys.Models;

public partial class Payment
{
    public int PaymentId { get; set; }

    public int? SaleId { get; set; }

    public string? ProcessorName { get; set; }

    public string? AuthorizationRequestId { get; set; }

    public string? AuthorizationCode { get; set; }

    public decimal? Amount { get; set; }

    public string? Currency { get; set; }

    public string? PaymentStatus { get; set; }

    public DateTime? RequestedAt { get; set; }

    public DateTime? RespondedAt { get; set; }

    public virtual Sale? Sale { get; set; }
}
