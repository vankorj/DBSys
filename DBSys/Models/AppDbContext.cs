using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DBSys.Models
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options)
			: base(options)
		{
		}

		// ======================
		// DBSYS TABLES
		// ======================

		public DbSet<Customer> Customers { get; set; }
		public DbSet<CustomersHistory> CustomersHistories { get; set; }
		public DbSet<OrderStatusRef> OrderStatusRefs { get; set; }

		public DbSet<Payment> Payments { get; set; }

		public DbSet<Product> Products { get; set; }
		public DbSet<ProductsHistory> ProductsHistories { get; set; }

		public DbSet<RevenueReport> RevenueReports { get; set; }

		public DbSet<Sale> Sales { get; set; }
		public DbSet<SalesHistory> SalesHistories { get; set; }

		public DbSet<Vendor> Vendors { get; set; }

		// ======================
		// MODEL CONFIGURATION
		// ======================

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			// ======================
			// CUSTOMER
			// ======================
			modelBuilder.Entity<Customer>(entity =>
			{
				entity.HasKey(e => e.CustomerId);
				entity.ToTable("CUSTOMERS", tb => tb.HasTrigger("trg_customers_history"));

				entity.Property(e => e.CustomerId).HasColumnName("customer_id");
				entity.Property(e => e.FirstName).HasColumnName("first_name").HasMaxLength(100).IsUnicode(false);
				entity.Property(e => e.LastName).HasColumnName("last_name").HasMaxLength(100).IsUnicode(false);
				entity.Property(e => e.Email).HasColumnName("email").HasMaxLength(200).IsUnicode(false);
				entity.Property(e => e.Phone).HasColumnName("phone");
				entity.Property(e => e.CreatedAt).HasColumnName("created_at");
			});

			// ======================
			// PRODUCT
			// ======================
			modelBuilder.Entity<Product>(entity =>
			{
				entity.HasKey(e => e.ProductId);
				entity.ToTable("PRODUCTS", tb => tb.HasTrigger("trg_products_history"));

				entity.Property(e => e.ProductId).HasColumnName("product_id");
				entity.Property(e => e.VendorId).HasColumnName("vendor_id");
				entity.Property(e => e.Sku).HasColumnName("sku").HasMaxLength(100).IsUnicode(false);
				entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(200).IsUnicode(false);
				entity.Property(e => e.Description).HasColumnName("description").HasMaxLength(500).IsUnicode(false);
				entity.Property(e => e.UnitPrice).HasColumnName("unit_price").HasColumnType("decimal(10,2)");
				entity.Property(e => e.Currency).HasColumnName("currency").HasMaxLength(10).IsUnicode(false);
				entity.Property(e => e.InventoryQty).HasColumnName("inventory_qty");
				entity.Property(e => e.IsActive).HasColumnName("is_active");
				entity.Property(e => e.CreatedAt).HasColumnName("created_at");

				entity.HasOne(p => p.Vendor)
					  .WithMany(v => v.Products)
					  .HasForeignKey(p => p.VendorId)
					  .OnDelete(DeleteBehavior.Restrict);
			});

			// ======================
			// VENDOR
			// ======================
			modelBuilder.Entity<Vendor>(entity =>
			{
				entity.HasKey(e => e.VendorId);
				entity.ToTable("VENDORS");

				entity.Property(e => e.VendorId).HasColumnName("vendor_id");
				entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(200).IsUnicode(false);
				entity.Property(e => e.ContactEmail).HasColumnName("contact_email");
				entity.Property(e => e.PayoutAccountRef).HasColumnName("payout_account_ref");
				entity.Property(e => e.CreatedAt).HasColumnName("created_at");
			});

			// ======================
			// SALE
			// ======================
			modelBuilder.Entity<Sale>(entity =>
			{
				entity.HasKey(e => e.SaleId);
				entity.ToTable("SALES", tb => tb.HasTrigger("trg_sales_history"));

				entity.Property(e => e.SaleId).HasColumnName("sale_id");
				entity.Property(e => e.CustomerId).HasColumnName("customer_id");
				entity.Property(e => e.ProductId).HasColumnName("product_id");
				entity.Property(e => e.VendorId).HasColumnName("vendor_id");
				entity.Property(e => e.Quantity).HasColumnName("quantity");
				entity.Property(e => e.UnitPriceAtSale).HasColumnName("unit_price_at_sale").HasColumnType("decimal(10,2)");
				entity.Property(e => e.SubtotalAmount).HasColumnName("subtotal_amount").HasColumnType("decimal(10,2)");
				entity.Property(e => e.TaxAmount).HasColumnName("tax_amount").HasColumnType("decimal(10,2)");
				entity.Property(e => e.TotalAmount).HasColumnName("total_amount").HasColumnType("decimal(10,2)");
				entity.Property(e => e.Currency).HasColumnName("currency");
				entity.Property(e => e.Status).HasColumnName("status");
				entity.Property(e => e.PurchasedAt).HasColumnName("purchased_at");

				entity.HasOne(s => s.Customer)
					  .WithMany(c => c.Sales)
					  .HasForeignKey(s => s.CustomerId);

				entity.HasOne(s => s.Product)
					  .WithMany()
					  .HasForeignKey(s => s.ProductId);

				entity.HasOne(s => s.Vendor)
					  .WithMany(v => v.Sales)
					  .HasForeignKey(s => s.VendorId);

				entity.HasOne(s => s.StatusNavigation)
					  .WithMany(o => o.Sales)
					  .HasForeignKey(s => s.Status)
					  .HasPrincipalKey(o => o.Status);
			});

			// ======================
			// PAYMENT
			// ======================
			modelBuilder.Entity<Payment>(entity =>
			{
				entity.HasKey(e => e.PaymentId);
				entity.ToTable("PAYMENTS");

				entity.Property(e => e.PaymentId).HasColumnName("payment_id");
				entity.Property(e => e.SaleId).HasColumnName("sale_id");
				entity.Property(e => e.ProcessorName).HasColumnName("processor_name");
				entity.Property(e => e.AuthorizationRequestId).HasColumnName("authorization_request_id");
				entity.Property(e => e.AuthorizationCode).HasColumnName("authorization_code");
				entity.Property(e => e.Amount).HasColumnName("amount").HasColumnType("decimal(10,2)");
				entity.Property(e => e.Currency).HasColumnName("currency");
				entity.Property(e => e.RequestedAt).HasColumnName("requested_at");
				entity.Property(e => e.RespondedAt).HasColumnName("responded_at");
				entity.Property(e => e.PaymentStatus).HasColumnName("payment_status");

				entity.HasOne(p => p.Sale)
					  .WithOne(s => s.Payment)
					  .HasForeignKey<Payment>(p => p.SaleId);
			});

			// ======================
			// PRODUCTS HISTORY
			// ======================
			modelBuilder.Entity<ProductsHistory>(entity =>
			{
				entity.HasKey(e => e.HistoryId);
				entity.ToTable("PRODUCTS_HISTORY");

				entity.Property(e => e.HistoryId).HasColumnName("history_id");
				entity.Property(e => e.ProductId).HasColumnName("product_id");
				entity.Property(e => e.Name).HasColumnName("name");
				entity.Property(e => e.UnitPrice).HasColumnName("unit_price").HasColumnType("decimal(10,2)");
				entity.Property(e => e.InventoryQty).HasColumnName("inventory_qty");
				entity.Property(e => e.AuditAction).HasColumnName("audit_action");
				entity.Property(e => e.AuditDate).HasColumnName("audit_date");
			});

			// ======================
			// SALES HISTORY
			// ======================
			modelBuilder.Entity<SalesHistory>(entity =>
			{
				entity.HasKey(e => e.HistoryId);
				entity.ToTable("SALES_HISTORY");

				entity.Property(e => e.HistoryId).HasColumnName("history_id");
				entity.Property(e => e.SaleId).HasColumnName("sale_id");
				entity.Property(e => e.TotalAmount).HasColumnName("total_amount").HasColumnType("decimal(10,2)");
				entity.Property(e => e.Status).HasColumnName("status");
				entity.Property(e => e.AuditAction).HasColumnName("audit_action");
				entity.Property(e => e.AuditDate).HasColumnName("audit_date");
			});

			// ======================
			// CUSTOMERS HISTORY
			// ======================
			modelBuilder.Entity<CustomersHistory>(entity =>
			{
				entity.HasKey(e => e.HistoryId);
				entity.ToTable("CUSTOMERS_HISTORY");

				entity.Property(e => e.HistoryId).HasColumnName("history_id");
				entity.Property(e => e.CustomerId).HasColumnName("customer_id");
				entity.Property(e => e.FirstName).HasColumnName("first_name");
				entity.Property(e => e.LastName).HasColumnName("last_name");
				entity.Property(e => e.Email).HasColumnName("email");
				entity.Property(e => e.Phone).HasColumnName("phone");
				entity.Property(e => e.AuditAction).HasColumnName("audit_action");
				entity.Property(e => e.AuditDate).HasColumnName("audit_date");
			});

			// ======================
			// REVENUE REPORT
			// ======================
			modelBuilder.Entity<RevenueReport>(entity =>
			{
				entity.HasKey(e => e.ReportId);
				entity.ToTable("REVENUE_REPORTS");

				entity.Property(e => e.ReportId).HasColumnName("report_id");
				entity.Property(e => e.ReportDate).HasColumnName("report_date");
				entity.Property(e => e.GeneratedAt).HasColumnName("generated_at");
				entity.Property(e => e.TotalSalesCount).HasColumnName("total_sales_count");
				entity.Property(e => e.GrossRevenueAmount)
					  .HasColumnName("gross_revenue_amount")
					  .HasColumnType("decimal(12,2)");
				entity.Property(e => e.Currency)
					  .HasColumnName("currency")
					  .HasMaxLength(10)
					  .IsUnicode(false);
			});

			// ======================
			// ORDER STATUS REF
			// ======================
			modelBuilder.Entity<OrderStatusRef>(entity =>
			{
				entity.HasKey(e => e.Status);
				entity.ToTable("ORDER_STATUS_REF");

				entity.Property(e => e.Status).HasColumnName("status");
			});
		}
	}
}
