------------------------------------------------------------
-- STAR SCHEMA CREATION
------------------------------------------------------------

CREATE TABLE DimCustomer (
    CustomerId INT PRIMARY KEY,
    FirstName NVARCHAR(100),
    LastName NVARCHAR(100),
    Email NVARCHAR(200),
    Phone NVARCHAR(50),
    CreatedAt DATETIME
);

CREATE TABLE DimProduct (
    ProductId INT PRIMARY KEY,
    Name NVARCHAR(200),
    SKU NVARCHAR(50),
    Description NVARCHAR(MAX),
    UnitPrice DECIMAL(10,2),
    Currency NVARCHAR(10),
    InventoryQty INT,
    IsActive BIT,
    CreatedAt DATETIME
);

CREATE TABLE DimVendor (
    VendorId INT PRIMARY KEY,
    Name NVARCHAR(200),
    ContactEmail NVARCHAR(200),
    PayoutAccountRef NVARCHAR(200),
    CreatedAt DATETIME
);

CREATE TABLE DimTime (
    TimeId INT IDENTITY(1,1) PRIMARY KEY,
    Date DATE,
    Day INT,
    Month INT,
    Quarter INT,
    Year INT,
    DayOfWeek INT
);

CREATE TABLE FactSales (
    FactSalesId INT IDENTITY(1,1) PRIMARY KEY,
    SaleId INT,
    CustomerId INT,
    ProductId INT,
    VendorId INT,
    TimeId INT,
    Quantity INT,
    UnitPriceAtSale DECIMAL(10,2),
    SubtotalAmount DECIMAL(10,2),
    TaxAmount DECIMAL(10,2),
    TotalAmount DECIMAL(10,2),
    Currency NVARCHAR(10),
    Status NVARCHAR(50)
);

------------------------------------------------------------
-- FULL ETL LOAD (ALL LOADS IN ONE SCRIPT)
------------------------------------------------------------

-- Load Customers
INSERT INTO DimCustomer
SELECT customer_id, first_name, last_name, email, phone, created_at
FROM CUSTOMERS;

-- Load Products
INSERT INTO DimProduct
SELECT product_id, name, sku, description, unit_price, currency, inventory_qty, is_active, created_at
FROM PRODUCTS;

-- Load Vendors
INSERT INTO DimVendor
SELECT vendor_id, name, contact_email, payout_account_ref, created_at
FROM VENDORS;

-- Load Time Dimension
INSERT INTO DimTime (Date, Day, Month, Quarter, Year, DayOfWeek)
SELECT DISTINCT
    CAST(purchased_at AS DATE),
    DAY(purchased_at),
    MONTH(purchased_at),
    DATEPART(QUARTER, purchased_at),
    YEAR(purchased_at),
    DATEPART(WEEKDAY, purchased_at)
FROM SALES;

-- Load Fact Table
INSERT INTO FactSales (
    SaleId, CustomerId, ProductId, VendorId, TimeId,
    Quantity, UnitPriceAtSale, SubtotalAmount, TaxAmount, TotalAmount, Currency, Status
)
SELECT 
    s.sale_id,
    s.customer_id,
    s.product_id,
    s.vendor_id,
    t.TimeId,
    s.quantity,
    s.unit_price_at_sale,
    s.subtotal_amount,
    s.tax_amount,
    s.total_amount,
    s.currency,
    s.status
FROM SALES s
JOIN DimTime t ON CAST(s.purchased_at AS DATE) = t.Date;
