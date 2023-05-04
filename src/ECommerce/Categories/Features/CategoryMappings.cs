namespace ECommerce.Categories.Features;

using AutoMapper;
using CreatingCategory;

public class CategoryMappings: Profile
{
    public CategoryMappings()
    {
        CreateMap<CreateCategoryRequestDto, CreateCategory>();
        CreateMap<CreateCategory, Models.Category>();
        CreateMap<Models.Category, CreateCategoryResult>();
    }
}