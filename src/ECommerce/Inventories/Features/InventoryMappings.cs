namespace ECommerce.Inventories.Features;

using AddingProductToInventory;
using AutoMapper;
using DamagingProduct;
using Models;
using SellingProduct;

public class InventoryMappings : Profile
{
    public InventoryMappings()
    {
        CreateMap<AddProductToInventoryRequestDto, AddProductToInventory>();
        CreateMap<AddProductToInventory, InventoryItems>();
        CreateMap<AddProductToInventoryResult, AddProductToInventoryResponseDto>();

        CreateMap<SellProductRequestDto, SellProduct>();
        CreateMap<SellProduct, InventoryItems>();

        CreateMap<DamageProductRequestDto, DamageProduct>();
        CreateMap<DamageProduct, InventoryItems>();
    }
}
