namespace ECommerce.Data.Seed;

using BuildingBlocks.EFCore;
using Microsoft.EntityFrameworkCore;

public class ECommerceDataSeeder : IDataSeeder
{
    private readonly ECommerceDbContext _eCommerceDbContext;

    public ECommerceDataSeeder(ECommerceDbContext eCommerceDbContext)
    {
        _eCommerceDbContext = eCommerceDbContext;
    }

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
        if (!await _eCommerceDbContext.Categories.AnyAsync())
        {
            await _eCommerceDbContext.Categories.AddRangeAsync(InitialData.Categories);
            await _eCommerceDbContext.SaveChangesAsync();
        }
    }

    private async Task SeedInventoryAsync()
    {
        if (!await _eCommerceDbContext.Inventories.AnyAsync())
        {
            await _eCommerceDbContext.Inventories.AddRangeAsync(InitialData.Inventories);
            await _eCommerceDbContext.SaveChangesAsync();
        }
    }

    private async Task SeedProductAsync()
    {
        if (!await _eCommerceDbContext.Products.AnyAsync())
        {
            await _eCommerceDbContext.Products.AddRangeAsync(InitialData.Products);
            await _eCommerceDbContext.SaveChangesAsync();
        }
    }

    private async Task SeedInventoryItemsAsync()
    {
        if (!await _eCommerceDbContext.InventoryItems.AnyAsync())
        {
            await _eCommerceDbContext.InventoryItems.AddRangeAsync(InitialData.InventoryItems);
            await _eCommerceDbContext.SaveChangesAsync();
        }
    }

    private async Task SeedCustomerAsync()
    {
        if (!await _eCommerceDbContext.Customers.AnyAsync())
        {
            await _eCommerceDbContext.Customers.AddRangeAsync(InitialData.Customers);
            await _eCommerceDbContext.SaveChangesAsync();
        }
    }
}
