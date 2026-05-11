using DBSys.Models;
using DBSys.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DBSys.Pages
{
	[Authorize(Roles = "Admin")]
	public class AnalyticsModel : PageModel
	{
		private readonly AnalyticsService _service;

		public List<ProductDemandDto> ProductDemand { get; set; }
		public List<VendorPerformanceDto> VendorPerformance { get; set; }
		public List<LowInventoryDto> LowInventory { get; set; }

		public AnalyticsModel(AnalyticsService service)
		{
			_service = service;
		}

		public async Task OnGetAsync()
		{
			ProductDemand = await _service.GetProductDemand();
			VendorPerformance = await _service.GetVendorPerformance();
			LowInventory = await _service.GetLowInventory();
		}
	}
}