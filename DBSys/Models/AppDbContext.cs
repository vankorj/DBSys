using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DBSys.Models;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<CustomersHistory> CustomersHistories { get; set; }

    public virtual DbSet<OrderStatusRef> OrderStatusRefs { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }


    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductsHistory> ProductsHistories { get; set; }

    public virtual DbSet<RevenueReport> RevenueReports { get; set; }

    public virtual DbSet<Sale> Sales { get; set; }

    public virtual DbSet<SalesHistory> SalesHistories { get; set; }

    public virtual DbSet<Vendor> Vendors { get; set; }

	public DbSet<PaymentProcessor> PaymentProcessors { get; set; }

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=MNStateFair;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("PK__CUSTOMER__CD65CB85641742F6");

            entity.ToTable("CUSTOMERS", tb => tb.HasTrigger("trg_customers_history"));

            entity.HasIndex(e => e.Email, "UQ__CUSTOMER__AB6E6164E0B9494A").IsUnique();

            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Email)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("first_name");
            entity.Property(e => e.LastName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("last_name");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("phone");
        });

        modelBuilder.Entity<CustomersHistory>(entity =>
        {
            entity.HasKey(e => e.HistoryId).HasName("PK__CUSTOMER__096AA2E9F6957455");

            entity.ToTable("CUSTOMERS_HISTORY");

            entity.Property(e => e.HistoryId).HasColumnName("history_id");
            entity.Property(e => e.AuditAction)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("audit_action");
            entity.Property(e => e.AuditDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("audit_date");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.Email)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("first_name");
            entity.Property(e => e.LastName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("last_name");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("phone");
        });

        modelBuilder.Entity<OrderStatusRef>(entity =>
        {
            entity.HasKey(e => e.Status).HasName("PK__ORDER_ST__A858923DDCDCC7A2");

            entity.ToTable("ORDER_STATUS_REF");

            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("status");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PK__PAYMENTS__ED1FC9EAFEEEFCB6");

            entity.ToTable("PAYMENTS");

            entity.HasIndex(e => e.SaleId, "UQ__PAYMENTS__E1EB00B391643BF2").IsUnique();

            entity.Property(e => e.PaymentId).HasColumnName("payment_id");
            entity.Property(e => e.Amount)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("amount");
            entity.Property(e => e.AuthorizationCode)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("authorization_code");
            entity.Property(e => e.AuthorizationRequestId)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("authorization_request_id");
            entity.Property(e => e.Currency)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("currency");
            entity.Property(e => e.PaymentStatus)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("payment_status");
            entity.Property(e => e.ProcessorName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("processor_name");
            entity.Property(e => e.RequestedAt)
                .HasColumnType("datetime")
                .HasColumnName("requested_at");
            entity.Property(e => e.RespondedAt)
                .HasColumnType("datetime")
                .HasColumnName("responded_at");
            entity.Property(e => e.SaleId).HasColumnName("sale_id");

            entity.HasOne(d => d.Sale).WithOne(p => p.Payment)
                .HasForeignKey<Payment>(d => d.SaleId)
                .HasConstraintName("FK__PAYMENTS__sale_i__4AB81AF0");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__PRODUCTS__47027DF590902F38");

            entity.ToTable("PRODUCTS", tb => tb.HasTrigger("trg_products_history"));

            entity.HasIndex(e => e.Sku, "UQ__PRODUCTS__DDDF4BE72A645DBC").IsUnique();

            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Currency)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("currency");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.InventoryQty).HasColumnName("inventory_qty");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Sku)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("sku");
            entity.Property(e => e.UnitPrice)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("unit_price");
            entity.Property(e => e.VendorId).HasColumnName("vendor_id");

            entity.HasOne(d => d.Vendor).WithMany(p => p.Products)
                .HasForeignKey(d => d.VendorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PRODUCTS__vendor__412EB0B6");
        });

        modelBuilder.Entity<ProductsHistory>(entity =>
        {
            entity.HasKey(e => e.HistoryId).HasName("PK__PRODUCTS__096AA2E956B07CE0");

            entity.ToTable("PRODUCTS_HISTORY");

            entity.Property(e => e.HistoryId).HasColumnName("history_id");
            entity.Property(e => e.AuditAction)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("audit_action");
            entity.Property(e => e.AuditDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("audit_date");
            entity.Property(e => e.InventoryQty).HasColumnName("inventory_qty");
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.UnitPrice)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("unit_price");
        });

        modelBuilder.Entity<RevenueReport>(entity =>
        {
            entity.HasKey(e => e.ReportId).HasName("PK__REVENUE___779B7C584E6AC71A");

            entity.ToTable("REVENUE_REPORTS");

            entity.Property(e => e.ReportId).HasColumnName("report_id");
            entity.Property(e => e.Currency)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("currency");
            entity.Property(e => e.GeneratedAt)
                .HasColumnType("datetime")
                .HasColumnName("generated_at");
            entity.Property(e => e.GrossRevenueAmount)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("gross_revenue_amount");
            entity.Property(e => e.ReportDate).HasColumnName("report_date");
            entity.Property(e => e.TotalSalesCount).HasColumnName("total_sales_count");

            entity.HasMany(d => d.Sales).WithMany(p => p.Reports)
                .UsingEntity<Dictionary<string, object>>(
                    "RevenueReportSale",
                    r => r.HasOne<Sale>().WithMany()
                        .HasForeignKey("SaleId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__REVENUE_R__sale___5070F446"),
                    l => l.HasOne<RevenueReport>().WithMany()
                        .HasForeignKey("ReportId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__REVENUE_R__repor__4F7CD00D"),
                    j =>
                    {
                        j.HasKey("ReportId", "SaleId").HasName("PK__REVENUE___4985CC539B44C1C2");
                        j.ToTable("REVENUE_REPORT_SALES");
                        j.IndexerProperty<int>("ReportId").HasColumnName("report_id");
                        j.IndexerProperty<int>("SaleId").HasColumnName("sale_id");
                    });
        });

        modelBuilder.Entity<Sale>(entity =>
        {
            entity.HasKey(e => e.SaleId).HasName("PK__SALES__E1EB00B2D99DC8BE");

            entity.ToTable("SALES", tb => tb.HasTrigger("trg_sales_history"));

            entity.Property(e => e.SaleId).HasColumnName("sale_id");
            entity.Property(e => e.Currency)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("currency");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.PurchasedAt)
                .HasColumnType("datetime")
                .HasColumnName("purchased_at");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("status");
            entity.Property(e => e.SubtotalAmount)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("subtotal_amount");
            entity.Property(e => e.TaxAmount)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("tax_amount");
            entity.Property(e => e.TotalAmount)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("total_amount");
            entity.Property(e => e.UnitPriceAtSale)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("unit_price_at_sale");
            entity.Property(e => e.VendorId).HasColumnName("vendor_id");

            entity.HasOne(d => d.Customer).WithMany(p => p.Sales)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK__SALES__customer___440B1D61");

            entity.HasOne(d => d.StatusNavigation).WithMany(p => p.Sales)
                .HasForeignKey(d => d.Status)
                .HasConstraintName("FK__SALES__status__46E78A0C");

            entity.HasOne(d => d.Vendor).WithMany(p => p.Sales)
                .HasForeignKey(d => d.VendorId)
                .HasConstraintName("FK__SALES__vendor_id__45F365D3");
        });

        modelBuilder.Entity<SalesHistory>(entity =>
        {
            entity.HasKey(e => e.HistoryId).HasName("PK__SALES_HI__096AA2E9D68AFEF6");

            entity.ToTable("SALES_HISTORY");

            entity.Property(e => e.HistoryId).HasColumnName("history_id");
            entity.Property(e => e.AuditAction)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("audit_action");
            entity.Property(e => e.AuditDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("audit_date");
            entity.Property(e => e.SaleId).HasColumnName("sale_id");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("status");
            entity.Property(e => e.TotalAmount)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("total_amount");
        });

        modelBuilder.Entity<Vendor>(entity =>
        {
            entity.HasKey(e => e.VendorId).HasName("PK__VENDORS__0F7D2B78C9A201EC");

            entity.ToTable("VENDORS");

            entity.Property(e => e.VendorId).HasColumnName("vendor_id");
            entity.Property(e => e.ContactEmail)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("contact_email");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.PayoutAccountRef)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("payout_account_ref");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
