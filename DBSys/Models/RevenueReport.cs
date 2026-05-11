using System;
using System.Collections.Generic;

namespace DBSys.Models;

public partial class RevenueReport
{
	public int ReportId { get; set; }
	public DateTime? ReportDate { get; set; }
	public DateTime? GeneratedAt { get; set; }
	public int? TotalSalesCount { get; set; }
	public decimal? GrossRevenueAmount { get; set; }
	public string? Currency { get; set; }
}
