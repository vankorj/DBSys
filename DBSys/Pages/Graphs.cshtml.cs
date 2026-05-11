using DBSys.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DBSys.Pages
{
	public class GraphsModel : PageModel
	{
		private readonly AppDbContext _context;

		public GraphsModel(AppDbContext context)
		{
			_context = context;
		}

		// Chart Data
		public List<string> ProductNames { get; set; } = new();
		public List<int> InventoryQuantities { get; set; } = new();

		public List<string> VendorNames { get; set; } = new();
		public List<decimal> VendorRevenue { get; set; } = new();

		public List<string> RevenueDates { get; set; } = new();
		public List<decimal> RevenueAmounts { get; set; } = new();

		public async Task OnGetAsync()
		{
			// ⭐ INVENTORY CHART (DimProduct)
			var inventory = await _context.DimProduct
				.Select(p => new
				{
					p.Name,
					p.InventoryQty
				})
				.ToListAsync();

			ProductNames = inventory.Select(i => i.Name).ToList();
			InventoryQuantities = inventory.Select(i => i.InventoryQty).ToList();


			// ⭐ VENDOR REVENUE PIE (FactSales + DimVendor)
			var vendorData = await _context.FactSales
				.Join(_context.DimVendor,
					f => f.VendorId,
					v => v.VendorId,
					(f, v) => new { v.Name, f.TotalAmount })
				.GroupBy(x => x.Name)
				.Select(g => new
				{
					Vendor = g.Key,
					Revenue = g.Sum(x => x.TotalAmount)
				})
				.ToListAsync();

			VendorNames = vendorData.Select(v => v.Vendor).ToList();
			VendorRevenue = vendorData.Select(v => v.Revenue).ToList();

			// ⭐ SALES REVENUE TREND (FactSales + DimTime)
			var revenueTrend = await _context.FactSales
				.Join(_context.DimTime,
					f => f.TimeId,
					t => t.TimeId,
					(f, t) => new { t.Date, f.TotalAmount })
				.GroupBy(x => x.Date)
				.Select(g => new
				{
					Date = g.Key,
					Revenue = g.Sum(x => x.TotalAmount)
				})
				.OrderBy(x => x.Date)
				.ToListAsync();   // ⭐ IMPORTANT

			RevenueDates = revenueTrend
				.Select(r => r.Date.ToString("yyyy-MM-dd"))
				.ToList();

			RevenueAmounts = revenueTrend
				.Select(r => r.Revenue)
				.ToList();

		}
	}
}
