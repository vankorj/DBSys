namespace DBSys.Models
{
	public class Customers
	{
		public int customer_id { get; set; }
		public string first_name { get; set; }
		public string last_name { get; set; }
		public string email { get; set; }
		public string phone { get; set; }
		public DateTime created_at { get; set; }
	}
}
