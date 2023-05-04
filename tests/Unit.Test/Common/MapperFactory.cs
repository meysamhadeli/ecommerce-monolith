namespace Unit.Test.Common;

using AutoMapper;
using ECommerce.Categories.Features;
using ECommerce.Inventories.Features;
using ECommerce.Products.Features;

public static class MapperFactory
{
    public static IMapper Create()
    {
        var configurationProvider = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<CategoryMappings>();
            cfg.AddProfile<ProductMappings>();
            cfg.AddProfile<InventoryMappings>();
        });

        return configurationProvider.CreateMapper();
    }
}
