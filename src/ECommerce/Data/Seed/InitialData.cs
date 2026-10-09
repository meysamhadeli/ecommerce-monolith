namespace ECommerce.Data.Seed;

using Categories.Models;
using Customers.Models;
using Inventories.Enums;
using Inventories.Models;
using MassTransit;
using Products.Models;

public static class InitialData
{
    public static List<Category> Categories => new()
    {
        new() { Id = new Guid("3c5c0000-97c6-fc34-fc3c-08db322230c8"), Name = "Food" },
        new() { Id = new Guid("3c5c0000-97c6-fc34-fc3c-08db322230c9"), Name = "Technology" },
    };

    public static List<Inventory> Inventories => new()
    {
        new() { Id = new Guid("3c5c0000-97c6-fc34-fc3c-08db322230c4"), Name = "Central-Inventory" },
        new() { Id = new Guid("3c5c0000-97c6-fc34-fc3c-08db322230c5"), Name = "Inventory-22" },
    };

    public static List<Product> Products => new()
    {
        new()
        {
            Id = new Guid("3c5c0000-97c6-fc34-fcd3-08db322230c0"),
            Name = "Cake",
            Barcode = "1234567890",
            IsBreakable = true,
            CategoryId = new Guid("3c5c0000-97c6-fc34-fc3c-08db322230c8"),
            Price = 50000,
            ProfitMargin = 0,
            NetPrice = 50000,
            Description = "It's a Cake",
        },
        new()
        {
            Id = new Guid("3c5c0000-97c6-fc34-fcd3-08db322230c1"),
            Name = "Pizza",
            Barcode = "1234567891",
            IsBreakable = true,
            CategoryId = new Guid("3c5c0000-97c6-fc34-fc3c-08db322230c8"),
            Price = 60000,
            ProfitMargin = 0,
            NetPrice = 60000,
            Description = "It's a Pizza",
        },
        new()
        {
            Id = new Guid("3c5c0000-97c6-fc34-fcd3-08db322230c2"),
            Name = "Drink",
            Barcode = "1234567892",
            IsBreakable = true,
            CategoryId = new Guid("3c5c0000-97c6-fc34-fc3c-08db322230c8"),
            Price = 70000,
            ProfitMargin = 0,
            NetPrice = 70000,
            Description = "It's a Drink",
        },
        new()
        {
            Id = new Guid("3c5c0000-97c6-fc34-fcd3-08db322230c3"),
            Name = "Keyboard",
            Barcode = "1234567893",
            IsBreakable = true,
            CategoryId = new Guid("3c5c0000-97c6-fc34-fc3c-08db322230c9"),
            Price = 80000,
            ProfitMargin = 0,
            NetPrice = 80000,
            Description = "It's a Keyboard",
        },
    };

    public static List<InventoryItems> InventoryItems => new()
    {
        new()
        {
            Id = NewId.NextGuid(),
            InventoryId = new Guid("3c5c0000-97c6-fc34-fc3c-08db322230c4"),
            ProductId = new Guid("3c5c0000-97c6-fc34-fcd3-08db322230c0"),
            Quantity = 2,
            Status = ProductStatus.InStock,
        },
        new()
        {
            Id = NewId.NextGuid(),
            InventoryId = new Guid("3c5c0000-97c6-fc34-fc3c-08db322230c4"),
            ProductId = new Guid("3c5c0000-97c6-fc34-fcd3-08db322230c1"),
            Quantity = 1,
            Status = ProductStatus.InStock,
        },
        new()
        {
            Id = NewId.NextGuid(),
            InventoryId = new Guid("3c5c0000-97c6-fc34-fc3c-08db322230c4"),
            ProductId = new Guid("3c5c0000-97c6-fc34-fcd3-08db322230c2"),
            Quantity = 5,
            Status = ProductStatus.InStock,
        },
        new()
        {
            Id = NewId.NextGuid(),
            InventoryId = new Guid("3c5c0000-97c6-fc34-fc3c-08db322230c5"),
            ProductId = new Guid("3c5c0000-97c6-fc34-fcd3-08db322230c3"),
            Quantity = 4,
            Status = ProductStatus.InStock,
        },
        new()
        {
            Id = NewId.NextGuid(),
            InventoryId = new Guid("3c5c0000-97c6-fc34-fc3c-08db322230c5"),
            ProductId = new Guid("3c5c0000-97c6-fc34-fcd3-08db322230c3"),
            Quantity = 4,
            Status = ProductStatus.Sold,
        },
        new()
        {
            Id = NewId.NextGuid(),
            InventoryId = new Guid("3c5c0000-97c6-fc34-fc3c-08db322230c4"),
            ProductId = new Guid("3c5c0000-97c6-fc34-fcd3-08db322230c1"),
            Quantity = 3,
            Status = ProductStatus.Damaged,
        },
    };

    public static List<Customer> Customers => new()
    {
        new()
        {
            Id = new Guid("2c5c0000-97c6-fc34-fcd3-08db322230c0"),
            Name = "Admin",
            Mobile = "09360000000",
            Address = "Tehran - Tehran - Rey",
        },
        new()
        {
            Id = new Guid("2c5c0000-97c6-fc34-fcd3-08db322230c1"),
            Name = "User",
            Mobile = "09361111111",
            Address = "Tehran - Tehran - Mirdamad",
        },
    };
}
