namespace ECommerce.Inventories.Features;

using AddingProductToInventory;
using AutoMapper;
using BuildingBlocks.Core.Pagination;
using DamagingProduct;
using Dtos;
using GettingAllInventoryByPage;
using GettingAllInventoryItemsByPage;
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

        CreateMap<GetAllInventoryByPageResult, GetAllInventoryByPageResponseDto>();
        CreateMap<GetAllInventoryByPageRequestDto, GetAllInventoryByPage>();

        CreateMap<Inventory, InventoryDto>()
            .ConstructUsing(x =>
                new InventoryDto(x.Id, x.Name));

        CreateMap<PageList<Inventory>, PageList<InventoryDto>>();

        CreateMap<GetAllInventoryItemsByPageResult, GetAllInventoryItemsByPageResponseDto>();
        CreateMap<GetAllInventoryItemsByPageRequestDto, GetAllInventoryItemsByPage>();

        CreateMap<InventoryItems, InventoryItemsDto>()
            .ConstructUsing(x =>
                new InventoryItemsDto(x.Id, x.InventoryId, x.ProductId, x.Quantity, x.Status));

        CreateMap<PageList<InventoryItems>, PageList<InventoryItemsDto>>();
    }
}
