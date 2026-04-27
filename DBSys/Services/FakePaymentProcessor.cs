using DBSys.Models;

namespace DBSys.Services;

public class FakePaymentProcessor
{
	public PaymentResult Process(Payment payment)
	{
		var rand = new Random();

		// Example rule-based logic (more realistic than pure random)
		bool approved = payment.Amount < 50 || rand.Next(0, 100) < 70;

		return new PaymentResult
		{
			IsApproved = approved,
			AuthorizationCode = approved ? $"AUTH-{rand.Next(10000, 99999)}" : null,
			Message = approved ? "Approved" : "Declined"
		};
	}
}