namespace ECommerce.Products.Features;

using AutoMapper;
using BuildingBlocks.Core.Pagination;
using CreatingProduct;
using Dtos;
using GettingAllProductsByPage;
using Models;
using ValueObjects;

public class ProductMappings : Profile
{
    public ProductMappings()
    {
        CreateMap<CreateProductRequestDto, CreateProduct>();
        CreateMap<CreateProduct, Product>();
        CreateMap<Product, CreateProductResult>();

        CreateMap<GetProductsByPageResult, GetProductsByPageResponseDto>();
        CreateMap<GetProductsByPageRequestDto, GetProductsByPage>();

        CreateMap<Product, ProductDto>()
            .ConstructUsing(x =>
                new ProductDto(x.Id, x.Name, x.Barcode, x.Description, x.CategoryId, x.IsBreakable, x.Price,
                    x.ProfitMargin, x.NetPrice));

        CreateMap<IPageList<Product>, IPageList<ProductDto>>();
    }
}
