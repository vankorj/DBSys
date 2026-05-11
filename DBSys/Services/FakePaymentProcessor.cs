using System;

namespace DBSys.Services;

public class FakePaymentProcessor
{
	public (string status, string? authCode) Process(decimal amount)
	{
		var rand = new Random();

		bool approved = amount switch
		{
			< 20 => true,
			< 100 => rand.Next(0, 100) < 80,
			_ => rand.Next(0, 100) < 50
		};

		if (approved)
		{
			return ("Approved", $"AUTH-{rand.Next(10000, 99999)}");
		}

		return ("Declined", null);
	}
}