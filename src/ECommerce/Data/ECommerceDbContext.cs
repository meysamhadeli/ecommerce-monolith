namespace ECommerce.Data;

using BuildingBlocks.EFCore;
using Categories.Models;
using Customers.Models;
using Inventories.Models;
using Microsoft.EntityFrameworkCore;
using Orders.Models;
using Orders.ValueObjects;
using Products.Models;

public sealed class ECommerceDbContext : AppDbContextBase
{
    public ECommerceDbContext(DbContextOptions<ECommerceDbContext> options) : base(options)
    {
    }
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Inventory> Inventories => Set<Inventory>();
    public DbSet<InventoryItems> InventoryItems => Set<InventoryItems>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<Customer> Customers => Set<Customer>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(EcommerceRoot).Assembly);
        builder.FilterSoftDeletedProperties();
    }
}
