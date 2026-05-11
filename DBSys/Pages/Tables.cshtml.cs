using DBSys.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace DBSys.Pages
{
	[Authorize]
	public class TablesModel : Microsoft.AspNetCore.Mvc.RazorPages.PageModel
	{
		private readonly AppDbContext _context;

		public TablesModel(AppDbContext context)
		{
			_context = context;
		}

		public List<Product> Products { get; set; } = new();
		public List<Vendor> Vendors { get; set; } = new();
		public List<Customer> Customers { get; set; } = new();
		public List<Payment> Payments { get; set; } = new();
		public List<Sale> Sales { get; set; } = new();

		public void OnGet()
		{
			Products = _context.Products.ToList();
			Vendors = _context.Vendors.ToList();
			Customers = _context.Customers.ToList();
			Payments = _context.Payments.ToList();
			Sales = _context.Sales.ToList();
		}
	}
}