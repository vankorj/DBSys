public class FactSales
{
	public int FactSalesId { get; set; }
	public int SaleId { get; set; }
	public int CustomerId { get; set; }
	public int ProductId { get; set; }
	public int VendorId { get; set; }
	public int TimeId { get; set; }

	public int Quantity { get; set; }
	public decimal UnitPriceAtSale { get; set; }
	public decimal SubtotalAmount { get; set; }
	public decimal TaxAmount { get; set; }
	public decimal TotalAmount { get; set; }
	public string Currency { get; set; }
	public string Status { get; set; }
}
