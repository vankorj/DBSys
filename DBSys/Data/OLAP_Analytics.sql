/* ============================================================
   1. PRODUCT DEMAND ANALYSIS (Matches ProductDemandDto)
   ============================================================ */
CREATE OR ALTER VIEW vw_ProductDemandTrend AS
SELECT 
    p.name AS Product,
    COUNT(s.sale_id) AS TotalOrders
FROM SALES s
JOIN PRODUCTS p ON s.product_id = p.product_id
GROUP BY p.name;
GO


/* ============================================================
   2. CUSTOMER ANALYSIS (Optional DTO, cleaned for consistency)
   ============================================================ */
CREATE OR ALTER VIEW vw_CustomerAnalysis AS
SELECT 
    c.first_name AS FirstName,
    c.last_name AS LastName,
    COUNT(s.sale_id) AS TotalOrders,
    SUM(s.total_amount) AS TotalSpent
FROM SALES s
JOIN CUSTOMERS c ON s.customer_id = c.customer_id
GROUP BY c.first_name, c.last_name;
GO


/* ============================================================
   3. VENDOR PERFORMANCE (Matches VendorPerformanceDto)
   ============================================================ */
CREATE OR ALTER VIEW vw_VendorPerformance AS
SELECT 
    v.vendor_id AS VendId,
    v.name AS Vendor,
    COUNT(s.sale_id) AS TotalOrders
FROM SALES s
JOIN PRODUCTS p ON s.product_id = p.product_id
JOIN VENDORS v ON p.vendor_id = v.vendor_id
GROUP BY v.vendor_id, v.name;
GO


/* ============================================================
   4. REVENUE SUMMARY (OLAP Fact Summary)
   ============================================================ */
CREATE OR ALTER VIEW vw_RevenueSummary AS
SELECT 
    SUM(total_amount) AS TotalRevenue,
    COUNT(sale_id) AS TotalTransactions,
    AVG(total_amount) AS AvgSaleValue
FROM SALES;
GO


/* ============================================================
   5. PAYMENT ANALYSIS
   ============================================================ */
CREATE OR ALTER VIEW vw_PaymentAnalysis AS
SELECT 
    COUNT(payment_id) AS TotalPayments,
    SUM(amount) AS TotalPaid
FROM PAYMENTS;
GO


/* ============================================================
   6. SALES TREND (Time‑Series Analysis)
   ============================================================ */
CREATE OR ALTER VIEW vw_SalesTrend AS
SELECT 
    CAST(purchased_at AS DATE) AS SaleDate,
    COUNT(sale_id) AS TotalOrders,
    SUM(total_amount) AS TotalRevenue
FROM SALES
GROUP BY CAST(purchased_at AS DATE);
GO


/* ============================================================
   7. LOW INVENTORY RISK (Matches LowInventoryDto)
   ============================================================ */
CREATE OR ALTER VIEW vw_LowInventory AS
SELECT 
    name AS Product,
    inventory_qty AS QuantityOnHand
FROM PRODUCTS
WHERE inventory_qty < 10;   -- threshold adjustable
GO
