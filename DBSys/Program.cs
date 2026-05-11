using DBSys.Models;
using DBSys.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//
// ======================
// DATABASES
// ======================
// OG application database (your existing DB)
builder.Services.AddDbContext<AppDbContext>(options =>
	options.UseSqlServer(
		builder.Configuration.GetConnectionString("DefaultConnection")));

//
// Identity database (NEW separate DB)
//
builder.Services.AddDbContext<AuthDbContext>(options =>
	options.UseSqlServer(
		builder.Configuration.GetConnectionString("IdentityConnection")));

//
// ======================
// IDENTITY CONFIG
// ======================
// Uses AuthDbContext (IMPORTANT FIX)
builder.Services
	.AddIdentity<IdentityUser, IdentityRole>(options =>
	{
		options.Password.RequireDigit = false;
		options.Password.RequireUppercase = false;
		options.Password.RequireNonAlphanumeric = false;
		options.Password.RequiredLength = 6;

		options.SignIn.RequireConfirmedAccount = false;
	})
	.AddEntityFrameworkStores<AuthDbContext>()
	.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
	options.LoginPath = "/Identity/Account/Login";
	options.AccessDeniedPath = "/Identity/Account/Login";
});

//
// ======================
// CUSTOM SERVICES
// ======================
builder.Services.AddScoped<FakePaymentProcessor>();

builder.Services.AddScoped<AnalyticsService>();

//
// ======================
// RAZOR PAGES
// ======================
builder.Services.AddRazorPages();

var app = builder.Build();

//
// ======================
// SEED DATA (ROLES / ADMIN)
// ======================
using (var scope = app.Services.CreateScope())
{
	var services = scope.ServiceProvider;

	// MUST target Identity DB via AuthDbContext internally
	await SeedData.Initialize(services);
}

//
// ======================
// PIPELINE
// ======================
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error");
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/test-sale", async (AppDbContext db, FakePaymentProcessor fp) =>
{
	var sale = new Sale
	{
		CustomerId = 1,
		ProductId = 1,
		VendorId = 1,
		Quantity = 2,
		UnitPriceAtSale = 10,
		SubtotalAmount = 20,
		TaxAmount = 1.5m,
		TotalAmount = 21.5m,
		Currency = "USD",
		PurchasedAt = DateTime.UtcNow
	};

	// ⭐ Call the processor to get the real status
	var (status, authCode) = fp.Process(sale.TotalAmount.Value);

	sale.Status = status;       // "Approved" or "Declined"

	db.Sales.Add(sale);
	await db.SaveChangesAsync();

	// ⭐ Now decrement inventory
	await fp.ProcessPaymentAsync(sale, db);

	return $"Sale processed. Status: {status}";
});

app.MapRazorPages();

app.Run();