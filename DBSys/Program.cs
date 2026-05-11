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

app.MapRazorPages();

app.Run();