USE CST2550SupermarketDb;
GO

INSERT INTO Categories (CategoryId, CategoryName) VALUES
(1, 'Dairy'),
(2, 'Bakery'),
(3, 'Groceries');

INSERT INTO Suppliers (SupplierId, SupplierName, ContactEmail, ContactPhone) VALUES
(1, 'FreshFarm Ltd', 'orders@freshfarm.co.uk', '02070000001'),
(2, 'London Bakery Supplies', 'sales@londonbakery.co.uk', '02070000002'),
(3, 'Global Foods', 'stock@globalfoods.co.uk', '02070000003');

INSERT INTO Products (ProductId, Barcode, Title, Category, SupplierName, CategoryId, SupplierId, ExpiryOrRestockDate, StockStatus, Price, QuantityInStock) VALUES
(1, '1001', 'Milk', 'Dairy', 'FreshFarm Ltd', 1, 1, DATEADD(day, 7, GETDATE()), 'Available', 1.80, 20),
(2, '1002', 'Bread', 'Bakery', 'London Bakery Supplies', 2, 2, DATEADD(day, 3, GETDATE()), 'Low Stock', 1.20, 4),
(3, '1003', 'Rice', 'Groceries', 'Global Foods', 3, 3, DATEADD(month, 6, GETDATE()), 'Available', 5.50, 15);

INSERT INTO Stock (ProductId, QuantityInStock, LastUpdated) VALUES
(1, 20, GETDATE()),
(2, 4, GETDATE()),
(3, 15, GETDATE());

INSERT INTO Sales (SaleDate, TotalAmount) VALUES
(GETDATE(), 3.60);

INSERT INTO SaleItems (SaleId, ProductId, ProductTitle, QuantitySold, UnitPrice, LineTotal) VALUES
(1, 1, 'Milk', 2, 1.80, 3.60);
