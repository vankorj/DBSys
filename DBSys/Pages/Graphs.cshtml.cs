using DBSys.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace DBSys.Pages
{
	[Authorize]
	public class GraphsModel : PageModel
	{
		private readonly AppDbContext _context;

		public GraphsModel(AppDbContext context)
		{
			_context = context;
		}

		// Inventory Chart
		public List<string> ProductNames { get; set; } = new();
		public List<int> InventoryQuantities { get; set; } = new();

		// Revenue Trend Chart
		public List<string> RevenueDates { get; set; } = new();
		public List<decimal> RevenueAmounts { get; set; } = new();

		// Vendor Revenue Chart
		public List<string> VendorNames { get; set; } = new();
		public List<decimal> VendorRevenue { get; set; } = new();

		public void OnGet()
		{
			// INVENTORY BAR CHART
			var products = _context.Products.ToList();

			ProductNames = products.Select(p => p.Name).ToList();
			InventoryQuantities = products
				.Select(p => p.InventoryQty ?? 0)
				.ToList();

			// SALES REVENUE LINE GRAPH
			var revenueData = _context.Sales
				.Where(s => s.PurchasedAt.HasValue)
				.GroupBy(s => s.PurchasedAt.Value.Date)
				.Select(g => new
				{
					Date = g.Key,
					Revenue = g.Sum(x => x.TotalAmount)
				})
				.OrderBy(x => x.Date)
				.ToList();

			RevenueDates = revenueData
				.Select(x => x.Date.ToString("MM/dd/yyyy"))
				.ToList();

			RevenueAmounts = revenueData
				.Select(x => x.Revenue ?? 0m)
				.ToList();

			// REVENUE BY VENDOR PIE CHART
			var vendorData = _context.Sales
				.Include(s => s.Vendor)
				.GroupBy(s => s.Vendor.Name)
				.Select(g => new
				{
					Vendor = g.Key,
					Revenue = g.Sum(x => x.TotalAmount)
				})
				.ToList();

			VendorNames = vendorData
				.Select(x => x.Vendor)
				.ToList();

			VendorRevenue = vendorData
				.Select(x => x.Revenue ?? 0m)
				.ToList();
		}
	}
}