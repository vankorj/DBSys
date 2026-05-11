using DBSys.Models;
using Microsoft.EntityFrameworkCore;

namespace DBSys.Services
{
	public class AnalyticsService
	{
		private readonly AppDbContext _context;

		public AnalyticsService(AppDbContext context)
		{
			_context = context;
		}

		public async Task<List<ProductDemandDto>> GetProductDemand()
		{
			return await _context.Set<ProductDemandDto>()
				.FromSqlRaw("SELECT * FROM vw_ProductDemandTrend")
				.ToListAsync();
		}

		public async Task<List<VendorPerformanceDto>> GetVendorPerformance()
		{
			return await _context.Set<VendorPerformanceDto>()
				.FromSqlRaw("SELECT * FROM vw_VendorPerformance")
				.ToListAsync();
		}

		public async Task<List<LowInventoryDto>> GetLowInventory()
		{
			return await _context.Set<LowInventoryDto>()
				.FromSqlRaw("SELECT * FROM vw_LowInventory")
				.ToListAsync();
		}
	}
}