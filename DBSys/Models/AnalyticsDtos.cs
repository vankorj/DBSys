using Microsoft.EntityFrameworkCore;

namespace DBSys.Models
{
	[Keyless]
	public class ProductDemandDto
	{
		public string Product { get; set; }
		public int TotalOrders { get; set; }
	}

	[Keyless]
	public class VendorPerformanceDto
	{
		public int VendId { get; set; }
		public string Vendor { get; set; }
		public int TotalOrders { get; set; }
	}

	[Keyless]
	public class LowInventoryDto
	{
		public string Product { get; set; }
		public int QuantityOnHand { get; set; }
	}
}
