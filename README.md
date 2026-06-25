# CST2550 Supermarket Management System

**Student:** Thriambakeshwar Manjunath
**Student ID:** M01046020
**Module:** CST2550 Reset Coursework
**Project:** Local Supermarket Management System for Small Shops

## Project overview

This project is a C# .NET console application designed for a small local supermarket or shop. The aim of the system is to replace a simple paper-based process with a digital system that can manage products, suppliers, stock levels and sales.

The application allows the user to add and manage products, maintain supplier records, update stock, record sales, search for products and generate reports. The program also includes custom data structures, searching algorithms, SQL Server / Entity Framework Core support, validation and unit tests.

The system is written as a console application so that it is simple to run and easy for a small shop worker to understand.

## Main features

* Add, edit, remove and view products
* Add, edit, remove and view suppliers
* Update stock quantities
* Restock existing products
* Record a sale and reduce stock automatically
* Show low-stock products
* Search products by name using linear search
* Search products by barcode using binary search
* Sort and display product information
* Generate stock, category, supplier and sales reports
* Validate user input such as missing fields, duplicate barcodes, negative stock and invalid prices
* Run with SQL Server LocalDB when available
* Fall back to sample in-memory data if LocalDB is not available

## Technologies used

* C# .NET 8
* Console application
* Entity Framework Core
* SQL Server LocalDB / SQL Server Express
* xUnit for unit testing
* Git and GitHub for version control

## Data structures and algorithms

The project includes custom data structures instead of only depending on built-in collections for the main implementation.

Custom structures used include:

* Product linked list
* Supplier linked list
* Sale linked list

Algorithms used include:

* Linear search for searching products by name
* Binary search for searching products by barcode
* Sorting logic for preparing ordered product data
* Stock and report calculations

## Folder structure

```text
CST2550-Supermarket-Management-System/
├── SupermarketManagementSystem/
│   ├── Algorithms/
│   ├── Data/
│   ├── DataStructures/
│   ├── Helpers/
│   ├── Models/
│   ├── Services/
│   └── Program.cs
├── SupermarketManagementSystem.Tests/
├── Database/
├── Report/
├── Video/
├── README.md
├── .gitignore
├── CST2550SupermarketSystem.sln
└── M01046020.txt
```

## Requirements

To run the project, the following are recommended:

* .NET 8 SDK
* Visual Studio 2022 or VS Code
* SQL Server LocalDB or SQL Server Express for database mode
* PowerShell or Command Prompt

## How to run the project

Open PowerShell in the root project folder and run:

```bash
dotnet restore
dotnet build
dotnet test
dotnet run --project SupermarketManagementSystem
```

If `dotnet` is not added to the system path, use the full path to the .NET SDK executable instead.

## Database information

The project includes SQL Server / Entity Framework Core support. The default connection string is stored in `appsettings.json` and points to LocalDB:

```text
Server=(localdb)\MSSQLLocalDB;Database=CST2550SupermarketDb;Trusted_Connection=True;TrustServerCertificate=True;
```

At startup, the program tries to create and seed the database using `DatabaseSeeder`. If LocalDB is not available on the machine, the program does not crash. It loads a sample in-memory data set so that the menu, searches, stock updates, sales and reports can still be demonstrated.

## SQL scripts

The `Database` folder contains:

* `create_database.sql` - creates the database tables and relationships
* `seed_data.sql` - inserts sample data for products, suppliers, stock and sales

These scripts are included to show the database structure separately from the Entity Framework Core code.

## Unit tests

The test project uses xUnit. The tests cover the main parts of the system, including:

* Product validation
* Duplicate barcode checks
* Linear search
* Binary search
* Stock updates
* Sale recording
* Report generation

Run the tests with:

```bash
dotnet test
```

## Report

The `Report` folder contains the coursework report in PDF format. The report explains the system design, data structures, algorithms, database design, testing and limitations.

## Demo video

The `Video` folder contains the demo video guide or video link file. The demo shows the application running from the console, including product management, supplier management, searching, stock updates, sales, reports and unit tests.

## Limitations and future improvements

The current system works as a simple console-based supermarket management system. Some possible future improvements include:

* A graphical user interface
* More advanced sales basket handling
* User login and staff roles
* Exporting reports to PDF or Excel
* Stronger database reporting features
* More detailed stock history tracking

## Submission note

The Moodle submission requires a text file containing the public GitHub repository URL. The file is named `M01046020.txt`.

The public GitHub repository link is included inside `M01046020.txt`.