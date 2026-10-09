namespace Unit.Test.Common;

using ECommerce.Data;
using ECommerce.Data.Seed;
using Microsoft.EntityFrameworkCore;

public static class DbContextFactory
{
    public static ECommerceDbContext Create()
    {
        var options = new DbContextOptionsBuilder<ECommerceDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new ECommerceDbContext(options);

        context.Categories.AddRange(InitialData.Categories);
        context.Inventories.AddRange(InitialData.Inventories);
        context.Products.AddRange(InitialData.Products);
        context.InventoryItems.AddRange(InitialData.InventoryItems);
        context.Customers.AddRange(InitialData.Customers);

        context.SaveChanges();

        return context;
    }

    public static void Destroy(ECommerceDbContext context)
    {
        context.Database.EnsureDeleted();

        context.Dispose();
    }
}
