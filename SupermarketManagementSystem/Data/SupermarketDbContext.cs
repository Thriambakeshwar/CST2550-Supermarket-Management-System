using Microsoft.EntityFrameworkCore;
using SupermarketManagementSystem.Models;

namespace SupermarketManagementSystem.Data;

public class SupermarketDbContext : DbContext
{
    private const string DefaultConnection = "Server=(localdb)\\MSSQLLocalDB;Database=CST2550SupermarketDb;Trusted_Connection=True;TrustServerCertificate=True;";
    private readonly string? connectionString;

    public SupermarketDbContext()
    {
        connectionString = DefaultConnection;
    }

    public SupermarketDbContext(string connectionString)
    {
        this.connectionString = connectionString;
    }

    public SupermarketDbContext(DbContextOptions<SupermarketDbContext> options) : base(options)
    {
    }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<Supplier> Suppliers => Set<Supplier>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Stock> Stock => Set<Stock>();
    public DbSet<Sale> Sales => Set<Sale>();
    public DbSet<SaleItem> SaleItems => Set<SaleItem>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(connectionString ?? DefaultConnection);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        KeepCourseworkIdsManual(modelBuilder);
        AddUsefulIndexes(modelBuilder);
        SetMoneyColumns(modelBuilder);
        SetRelationships(modelBuilder);
    }

    private static void KeepCourseworkIdsManual(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>().Property(product => product.ProductId).ValueGeneratedNever();
        modelBuilder.Entity<Supplier>().Property(supplier => supplier.SupplierId).ValueGeneratedNever();
        modelBuilder.Entity<Category>().Property(category => category.CategoryId).ValueGeneratedNever();
    }

    private static void AddUsefulIndexes(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>().HasIndex(product => product.Barcode).IsUnique();
        modelBuilder.Entity<Category>().HasIndex(category => category.CategoryName).IsUnique();
        modelBuilder.Entity<Supplier>().HasIndex(supplier => supplier.SupplierName).IsUnique();
    }

    private static void SetMoneyColumns(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>().Property(product => product.Price).HasColumnType("decimal(10,2)");
        modelBuilder.Entity<Sale>().Property(sale => sale.TotalAmount).HasColumnType("decimal(10,2)");
        modelBuilder.Entity<SaleItem>().Property(item => item.UnitPrice).HasColumnType("decimal(10,2)");
        modelBuilder.Entity<SaleItem>().Property(item => item.LineTotal).HasColumnType("decimal(10,2)");
    }

    private static void SetRelationships(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>()
            .HasOne(product => product.CategoryRecord)
            .WithMany(category => category.Products)
            .HasForeignKey(product => product.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Product>()
            .HasOne(product => product.SupplierRecord)
            .WithMany(supplier => supplier.Products)
            .HasForeignKey(product => product.SupplierId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Stock>()
            .HasOne(stock => stock.Product)
            .WithOne(product => product.StockRecord)
            .HasForeignKey<Stock>(stock => stock.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Sale>()
            .HasMany(sale => sale.SaleItems)
            .WithOne(item => item.Sale)
            .HasForeignKey(item => item.SaleId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<SaleItem>()
            .HasOne(item => item.Product)
            .WithMany(product => product.SaleItems)
            .HasForeignKey(item => item.ProductId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
