using DBSys.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace DBSys.Pages
{
	[Authorize(Roles = "Admin")]
	public class HistoryModel : PageModel
	{
		private readonly AppDbContext _context;

		public HistoryModel(AppDbContext context)
		{
			_context = context;
		}

		public IList<CustomersHistory> CustomerHistory { get; set; }
		public IList<ProductsHistory> ProductHistory { get; set; }
		public IList<SalesHistory> SalesHistory { get; set; }

		public async Task OnGetAsync()
		{
			CustomerHistory = await _context.CustomersHistories
				.OrderByDescending(x => x.AuditDate)
				.ToListAsync();

			ProductHistory = await _context.ProductsHistories
				.OrderByDescending(x => x.AuditDate)
				.ToListAsync();

			SalesHistory = await _context.SalesHistories
				.OrderByDescending(x => x.AuditDate)
				.ToListAsync();
		}
	}
}