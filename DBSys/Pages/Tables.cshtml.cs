using DBSys.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace DBSys.Pages
{
	public class TablesModel : PageModel
	{
		private readonly AppDbContext _context;

		public TablesModel(AppDbContext context)
		{
			_context = context;
		}

		public List<Product> Products { get; set; }

		public async Task OnGetAsync()
		{
			Products = await _context.Products.ToListAsync();
		}
	}
}
