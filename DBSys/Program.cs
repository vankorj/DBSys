using DBSys.Models;
using DBSys.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//
// ======================
// DATABASES
// ======================
builder.Services.AddDbContext<AppDbContext>(options =>
	options.UseSqlServer(
		builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDbContext<AuthDbContext>(options =>
	options.UseSqlServer(
		builder.Configuration.GetConnectionString("IdentityConnection")));

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

builder.Services.AddScoped<FakePaymentProcessor>();

builder.Services.AddScoped<AnalyticsService>();

builder.Services.AddRazorPages();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
	var services = scope.ServiceProvider;

	await SeedData.Initialize(services);
}

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

	var (status, authCode) = fp.Process(sale.TotalAmount.Value);

	sale.Status = status;

	db.Sales.Add(sale);
	await db.SaveChangesAsync();

	await fp.ProcessPaymentAsync(sale, db);

	return $"Sale processed. Status: {status}";
});

app.MapRazorPages();

app.Run();