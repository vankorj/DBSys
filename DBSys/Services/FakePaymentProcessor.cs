using System;

public class FakePaymentProcessor
{
    public PaymentResult Process(decimal amount)
    {
        var rand = new Random();

        bool approved = rand.Next(0, 100) < 80; // 80% success rate

        return new PaymentResult
        {
            IsApproved = approved,
            AuthorizationCode = approved ? "AUTH-" + rand.Next(1000, 9999) : null,
            Message = approved ? "Approved" : "Declined"
        };
    }
}