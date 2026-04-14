CREATE DATABASE MNStateFair;
Use MNStateFair;

/* ============================
REFERENCE TABLE
============================ */

CREATE TABLE ORDER_STATUS_REF
(
    status VARCHAR(20) PRIMARY KEY
);

INSERT INTO ORDER_STATUS_REF VALUES
('Pending'),
('Confirmed'),
('Cancelled'),
('Refunded');


/* ============================
BASE TABLES
============================ */

CREATE TABLE CUSTOMERS
(
    customer_id INT IDENTITY PRIMARY KEY,
    first_name VARCHAR(100),
    last_name VARCHAR(100),
    email VARCHAR(200) UNIQUE,
    phone VARCHAR(20),
    created_at DATETIME DEFAULT GETDATE()
);

CREATE TABLE VENDORS
(
    vendor_id INT IDENTITY PRIMARY KEY,
    name VARCHAR(200),
    contact_email VARCHAR(200),
    payout_account_ref VARCHAR(200),
    created_at DATETIME DEFAULT GETDATE()
);

CREATE TABLE PRODUCTS
(
    product_id INT IDENTITY PRIMARY KEY,
    vendor_id INT NOT NULL,
    sku VARCHAR(100) UNIQUE,
    name VARCHAR(200),
    description VARCHAR(500),
    unit_price DECIMAL(10,2),
    currency VARCHAR(10),
    inventory_qty INT,
    is_active BIT,
    created_at DATETIME DEFAULT GETDATE(),

    FOREIGN KEY (vendor_id) REFERENCES VENDORS(vendor_id)
);

CREATE TABLE SALES
(
    sale_id INT IDENTITY PRIMARY KEY,
    customer_id INT,
    product_id INT,
    vendor_id INT,
    quantity INT,
    unit_price_at_sale DECIMAL(10,2),
    subtotal_amount DECIMAL(10,2),
    tax_amount DECIMAL(10,2),
    total_amount DECIMAL(10,2),
    currency VARCHAR(10),
    status VARCHAR(20),
    purchased_at DATETIME,

    FOREIGN KEY (customer_id) REFERENCES CUSTOMERS(customer_id),
    FOREIGN KEY (product_id) REFERENCES PRODUCTS(product_id),
    FOREIGN KEY (vendor_id) REFERENCES VENDORS(vendor_id),
    FOREIGN KEY (status) REFERENCES ORDER_STATUS_REF(status)
);

CREATE TABLE PAYMENTS
(
    payment_id INT IDENTITY PRIMARY KEY,
    sale_id INT UNIQUE,
    processor_name VARCHAR(100),
    authorization_request_id VARCHAR(100),
    authorization_code VARCHAR(100),
    amount DECIMAL(10,2),
    currency VARCHAR(10),
    payment_status VARCHAR(50),
    requested_at DATETIME,
    responded_at DATETIME,

    FOREIGN KEY (sale_id) REFERENCES SALES(sale_id)
);

CREATE TABLE REVENUE_REPORTS
(
    report_id INT IDENTITY PRIMARY KEY,
    report_date DATE,
    generated_at DATETIME,
    total_sales_count INT,
    gross_revenue_amount DECIMAL(12,2),
    currency VARCHAR(10)
);


/* ============================
JOIN TABLE
============================ */

CREATE TABLE REVENUE_REPORT_SALES
(
    report_id INT,
    sale_id INT,

    PRIMARY KEY (report_id, sale_id),

    FOREIGN KEY (report_id) REFERENCES REVENUE_REPORTS(report_id),
    FOREIGN KEY (sale_id) REFERENCES SALES(sale_id)
);




/* ============================
HISTORY TABLES
============================ */

CREATE TABLE CUSTOMERS_HISTORY
(
    history_id INT IDENTITY PRIMARY KEY,
    customer_id INT,
    first_name VARCHAR(100),
    last_name VARCHAR(100),
    email VARCHAR(200),
    phone VARCHAR(20),
    audit_action VARCHAR(10),
    audit_date DATETIME DEFAULT GETDATE()
);

CREATE TABLE PRODUCTS_HISTORY
(
    history_id INT IDENTITY PRIMARY KEY,
    product_id INT,
    name VARCHAR(200),
    unit_price DECIMAL(10,2),
    inventory_qty INT,
    audit_action VARCHAR(10),
    audit_date DATETIME DEFAULT GETDATE()
);

CREATE TABLE SALES_HISTORY
(
    history_id INT IDENTITY PRIMARY KEY,
    sale_id INT,
    total_amount DECIMAL(10,2),
    status VARCHAR(20),
    audit_action VARCHAR(10),
    audit_date DATETIME DEFAULT GETDATE()
);


/* ============================
TRIGGERS
============================ */

GO

CREATE TRIGGER trg_customers_history
ON CUSTOMERS
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
INSERT INTO CUSTOMERS_HISTORY
(customer_id, first_name, last_name, email, phone, audit_action)
SELECT customer_id, first_name, last_name, email, phone, 'INSERT'
FROM inserted;
END

GO

CREATE TRIGGER trg_products_history
ON PRODUCTS
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
INSERT INTO PRODUCTS_HISTORY
(product_id, name, unit_price, inventory_qty, audit_action)
SELECT product_id, name, unit_price, inventory_qty, 'INSERT'
FROM inserted;
END

GO

CREATE TRIGGER trg_sales_history
ON SALES
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
INSERT INTO SALES_HISTORY
(sale_id, total_amount, status, audit_action)
SELECT sale_id, total_amount, status, 'INSERT'
FROM inserted;
END
GO