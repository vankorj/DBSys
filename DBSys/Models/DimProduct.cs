public class DimProduct
{
	public int ProductId { get; set; }
	public string Name { get; set; }
	public string SKU { get; set; }
	public string Description { get; set; }
	public decimal UnitPrice { get; set; }
	public string Currency { get; set; }
	public int InventoryQty { get; set; }
	public bool IsActive { get; set; }
	public DateTime CreatedAt { get; set; }
}
