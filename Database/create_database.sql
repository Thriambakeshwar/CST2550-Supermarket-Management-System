CREATE DATABASE CST2550SupermarketDb;
GO

USE CST2550SupermarketDb;
GO

CREATE TABLE Categories (
    CategoryId INT PRIMARY KEY,
    CategoryName NVARCHAR(70) NOT NULL UNIQUE
);

CREATE TABLE Suppliers (
    SupplierId INT PRIMARY KEY,
    SupplierName NVARCHAR(100) NOT NULL UNIQUE,
    ContactEmail NVARCHAR(120) NOT NULL,
    ContactPhone NVARCHAR(30) NOT NULL
);

CREATE TABLE Products (
    ProductId INT PRIMARY KEY,
    Barcode NVARCHAR(30) NOT NULL UNIQUE,
    Title NVARCHAR(100) NOT NULL,
    Category NVARCHAR(70) NOT NULL,
    SupplierName NVARCHAR(100) NOT NULL,
    CategoryId INT NOT NULL,
    SupplierId INT NOT NULL,
    ExpiryOrRestockDate DATE NOT NULL,
    StockStatus NVARCHAR(30) NOT NULL,
    Price DECIMAL(10,2) NOT NULL CHECK (Price > 0),
    QuantityInStock INT NOT NULL CHECK (QuantityInStock >= 0),
    CONSTRAINT FK_Products_Categories FOREIGN KEY (CategoryId) REFERENCES Categories(CategoryId),
    CONSTRAINT FK_Products_Suppliers FOREIGN KEY (SupplierId) REFERENCES Suppliers(SupplierId)
);

CREATE TABLE Stock (
    StockId INT IDENTITY(1,1) PRIMARY KEY,
    ProductId INT NOT NULL UNIQUE,
    QuantityInStock INT NOT NULL CHECK (QuantityInStock >= 0),
    LastUpdated DATETIME2 NOT NULL,
    CONSTRAINT FK_Stock_Products FOREIGN KEY (ProductId) REFERENCES Products(ProductId) ON DELETE CASCADE
);

CREATE TABLE Sales (
    SaleId INT IDENTITY(1,1) PRIMARY KEY,
    SaleDate DATETIME2 NOT NULL,
    TotalAmount DECIMAL(10,2) NOT NULL CHECK (TotalAmount >= 0)
);

CREATE TABLE SaleItems (
    SaleItemId INT IDENTITY(1,1) PRIMARY KEY,
    SaleId INT NOT NULL,
    ProductId INT NOT NULL,
    ProductTitle NVARCHAR(100) NOT NULL,
    QuantitySold INT NOT NULL CHECK (QuantitySold > 0),
    UnitPrice DECIMAL(10,2) NOT NULL CHECK (UnitPrice > 0),
    LineTotal DECIMAL(10,2) NOT NULL CHECK (LineTotal >= 0),
    CONSTRAINT FK_SaleItems_Sales FOREIGN KEY (SaleId) REFERENCES Sales(SaleId) ON DELETE CASCADE,
    CONSTRAINT FK_SaleItems_Products FOREIGN KEY (ProductId) REFERENCES Products(ProductId)
);
