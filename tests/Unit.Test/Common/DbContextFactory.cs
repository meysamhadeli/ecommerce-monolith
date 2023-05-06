namespace Unit.Test.Common;

using ECommerce.Categories.Models;
using ECommerce.Categories.ValueObjects;
using ECommerce.Data;
using ECommerce.Inventories.Enums;
using ECommerce.Inventories.Models;
using ECommerce.Inventories.ValueObjects;
using ECommerce.Products.Models;
using ECommerce.Products.ValueObjects;
using MassTransit;
using CategoryName = ECommerce.Categories.ValueObjects.Name;
using ProductName = ECommerce.Products.ValueObjects.Name;
using InventoryName = ECommerce.Inventories.ValueObjects.Name;
using System;
using System.Collections.Generic;
using ECommerce.Customers.Models;
using ECommerce.Customers.ValueObjects;
using Microsoft.EntityFrameworkCore;

public static class DbContextFactory
{
    public static List<Category> Categories { get; private set; }
    public static List<Product> Products { get; private set;}
    public static List<Inventory> Inventories { get; private set;}
    public static List<InventoryItems> InventoryItems { get; private set;}
    public static List<Customer> Customers { get; private set;}

    public static ECommerceDbContext Create()
    {
        var options = new DbContextOptionsBuilder<ECommerceDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

        var context = new ECommerceDbContext(options);

        // Seed our data
        EcommerceDataSeeder(context);

        return context;
    }

    private static void EcommerceDataSeeder(ECommerceDbContext context)
    {
        Categories = new List<Category>
        {
            Category.Create(CategoryId.Of(new Guid("3c5c0000-97c6-fc34-fc3c-08db322230c8")),
                CategoryName.Of("Food")),
            Category.Create(CategoryId.Of(new Guid("3c5c0000-97c6-fc34-fc3c-08db322230c9")),
                CategoryName.Of("Technology")),
        };

        context.Categories.AddRange(Categories);

        Inventories = new List<Inventory>
        {
            Inventory.Create(InventoryId.Of(new Guid("3c5c0000-97c6-fc34-fc3c-08db322230c4")),
                InventoryName.Of("Central-Inventory")),
            Inventory.Create(InventoryId.Of(new Guid("3c5c0000-97c6-fc34-fc3c-08db322230c5")),
                InventoryName.Of("Inventory-22")),
        };

        context.Inventories.AddRange(Inventories);

        Products = new List<Product>
        {
            Product.Create(ProductId.Of(new Guid("3c5c0000-97c6-fc34-fcd3-08db322230c0")),
                ProductName.Of("Cake"), Barcode.Of("1234567890"), true,
                CategoryId.Of(new Guid("3c5c0000-97c6-fc34-fc3c-08db322230c8")), Price.Of(50000), ProfitMargin.Of(0),
                Description.Of("It's a Cake")),
            Product.Create(ProductId.Of(new Guid("3c5c0000-97c6-fc34-fcd3-08db322230c1")),
                ProductName.Of("Pizza"), Barcode.Of("1234567891"), true,
                CategoryId.Of(new Guid("3c5c0000-97c6-fc34-fc3c-08db322230c8")), Price.Of(60000), ProfitMargin.Of(0),
                Description.Of("It's a Pizza")),
            Product.Create(ProductId.Of(new Guid("3c5c0000-97c6-fc34-fcd3-08db322230c2")),
                ProductName.Of("Drink"), Barcode.Of("1234567892"), true,
                CategoryId.Of(new Guid("3c5c0000-97c6-fc34-fc3c-08db322230c8")), Price.Of(70000), ProfitMargin.Of(0),
                Description.Of("It's a Drink")),
            Product.Create(ProductId.Of(new Guid("3c5c0000-97c6-fc34-fcd3-08db322230c3")),
                ProductName.Of("Keyboard"), Barcode.Of("1234567893"), true,
                CategoryId.Of(new Guid("3c5c0000-97c6-fc34-fc3c-08db322230c9")), Price.Of(80000), ProfitMargin.Of(0),
                Description.Of("It's a Keyboard")),
        };

        context.Products.AddRange(Products);

        InventoryItems = new List<InventoryItems>
        {
            ECommerce.Inventories.Models.InventoryItems.AddProductToInventory(InventoryItemsId.Of(NewId.NextGuid()),
                InventoryId.Of(new Guid("3c5c0000-97c6-fc34-fc3c-08db322230c4")),
                ProductId.Of(new Guid("3c5c0000-97c6-fc34-fcd3-08db322230c0")), Quantity.Of(2)),
            ECommerce.Inventories.Models.InventoryItems.AddProductToInventory(InventoryItemsId.Of(NewId.NextGuid()),
                InventoryId.Of(new Guid("3c5c0000-97c6-fc34-fc3c-08db322230c4")),
                ProductId.Of(new Guid("3c5c0000-97c6-fc34-fcd3-08db322230c1")), Quantity.Of(1)),
            ECommerce.Inventories.Models.InventoryItems.AddProductToInventory(InventoryItemsId.Of(NewId.NextGuid()),
                InventoryId.Of(new Guid("3c5c0000-97c6-fc34-fc3c-08db322230c4")),
                ProductId.Of(new Guid("3c5c0000-97c6-fc34-fcd3-08db322230c2")), Quantity.Of(5)),
            ECommerce.Inventories.Models.InventoryItems.AddProductToInventory(InventoryItemsId.Of(NewId.NextGuid()),
                InventoryId.Of(new Guid("3c5c0000-97c6-fc34-fc3c-08db322230c5")),
                ProductId.Of(new Guid("3c5c0000-97c6-fc34-fcd3-08db322230c3")), Quantity.Of(4)),
            ECommerce.Inventories.Models.InventoryItems.AddProductToInventory(InventoryItemsId.Of(NewId.NextGuid()),
                InventoryId.Of(new Guid("3c5c0000-97c6-fc34-fc3c-08db322230c5")),
                ProductId.Of(new Guid("3c5c0000-97c6-fc34-fcd3-08db322230c3")), Quantity.Of(4), ProductStatus.Sold),
            ECommerce.Inventories.Models.InventoryItems.AddProductToInventory(InventoryItemsId.Of(NewId.NextGuid()),
                InventoryId.Of(new Guid("3c5c0000-97c6-fc34-fc3c-08db322230c4")),
                ProductId.Of(new Guid("3c5c0000-97c6-fc34-fcd3-08db322230c1")), Quantity.Of(3), ProductStatus.Damaged),
        };

        context.InventoryItems.AddRange(InventoryItems);

        Customers = new List<Customer>
        {
            Customer.Create(CustomerId.Of(new Guid("2c5c0000-97c6-fc34-fcd3-08db322230c0")), ECommerce.Customers.ValueObjects.Name.Of("Admin"), Mobile.Of("09360000000"), Address.Of("Tehran", "Tehran", "Rey") ),
            Customer.Create(CustomerId.Of(new Guid("2c5c0000-97c6-fc34-fcd3-08db322230c1")), ECommerce.Customers.ValueObjects.Name.Of("User"), Mobile.Of("09361111111"), Address.Of("Tehran", "Tehran", "Mirdamad"))
        };

        context.Customers.AddRange(Customers);

        context.SaveChanges();
    }

    public static void Destroy(ECommerceDbContext context)
    {
        context.Database.EnsureDeleted();
        context.Dispose();
    }
}
