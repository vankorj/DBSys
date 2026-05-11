using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DBSys.Models;
using DBSys.Services;

namespace DBSys.Pages.Payments;

public class IndexModel : PageModel
{
	private readonly AppDbContext _context;

	public IndexModel(AppDbContext context)
	{
		_context = context;
	}

	[BindProperty]
	public decimal Amount { get; set; }

	[BindProperty]
	public string ProcessorName { get; set; } = "FakeStripe";

	public List<Payment> Payments { get; set; } = new();

	public void OnGet()
	{
		Payments = _context.Payments
			.OrderByDescending(p => p.RequestedAt)
			.ToList();
	}

	public async Task<IActionResult> OnPostAsync()
	{
		var processor = new FakePaymentProcessor();

		// Step 1: Create request
		var payment = new Payment
		{
			ProcessorName = ProcessorName,
			AuthorizationRequestId = $"REQ-{Guid.NewGuid()}",
			Amount = Amount,
			Currency = "USD",
			RequestedAt = DateTime.Now,
			PaymentStatus = "Pending"
		};

		// Step 2: Process
		var result = processor.Process(payment.Amount ?? 0);

		// Step 3: Update response
		payment.PaymentStatus = result.status;
		payment.AuthorizationCode = result.authCode;
		payment.RespondedAt = DateTime.Now;

		// Step 4: Save
		_context.Payments.Add(payment);
		await _context.SaveChangesAsync();

		return RedirectToPage();
	}
}