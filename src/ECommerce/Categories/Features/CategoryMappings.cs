namespace ECommerce.Categories.Features;

using AutoMapper;
using BuildingBlocks.Core.Pagination;
using CreatingCategory;
using Dtos;
using GettingAllCategoriesByPage;
using Models;

public class CategoryMappings: Profile
{
    public CategoryMappings()
    {
        CreateMap<CreateCategoryRequestDto, CreateCategory>();
        CreateMap<CreateCategory, Category>();
        CreateMap<Category, CreateCategoryResult>();

        CreateMap<GetCategoriesByPageResult, GetCategoriesByPageResponseDto>();
        CreateMap<GetCategoriesByPageRequestDto, GetCategoriesByPage>();

        CreateMap<Category, CategoryDto>()
            .ConstructUsing(x =>
                new CategoryDto(x.Id, x.Name));

        CreateMap<PageList<Category>, PageList<CategoryDto>>();
    }
}
