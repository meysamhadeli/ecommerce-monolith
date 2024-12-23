using BuildingBlocks.EFCore;
using ECommerce.Data;
using ECommerce.Data.Seed;
using Microsoft.EntityFrameworkCore;
namespace EndToEnd.Test;

public class ECommerceTestDataSeeder(ECommerceDbContext eCommerceDbContext) : ITestDataSeeder
{
    public async Task SeedAllAsync()
    {
        await SeedCategoryAsync();
        await SeedInventoryAsync();
        await SeedProductAsync();
        await SeedInventoryItemsAsync();
        await SeedCustomerAsync();
    }

    private async Task SeedCategoryAsync()
    {
        if (!await eCommerceDbContext.Categories.AnyAsync())
        {
            await eCommerceDbContext.Categories.AddRangeAsync(InitialData.Categories);
            await eCommerceDbContext.SaveChangesAsync();
        }
    }

    private async Task SeedInventoryAsync()
    {
        if (!await eCommerceDbContext.Inventories.AnyAsync())
        {
            await eCommerceDbContext.Inventories.AddRangeAsync(InitialData.Inventories);
            await eCommerceDbContext.SaveChangesAsync();
        }
    }

    private async Task SeedProductAsync()
    {
        if (!await eCommerceDbContext.Products.AnyAsync())
        {
            await eCommerceDbContext.Products.AddRangeAsync(InitialData.Products);
            await eCommerceDbContext.SaveChangesAsync();
        }
    }

    private async Task SeedInventoryItemsAsync()
    {
        if (!await eCommerceDbContext.InventoryItems.AnyAsync())
        {
            await eCommerceDbContext.InventoryItems.AddRangeAsync(InitialData.InventoryItems);
            await eCommerceDbContext.SaveChangesAsync();
        }
    }

    private async Task SeedCustomerAsync()
    {
        if (!await eCommerceDbContext.Customers.AnyAsync())
        {
            await eCommerceDbContext.Customers.AddRangeAsync(InitialData.Customers);
            await eCommerceDbContext.SaveChangesAsync();
        }
    }
}
