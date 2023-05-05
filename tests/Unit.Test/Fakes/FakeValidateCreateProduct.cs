namespace Unit.Test.Fakes;

using AutoBogus;
using ECommerce.Products.Features.CreatingProduct;

public class FakeValidateCreateProduct : AutoFaker<CreateProduct>
{
    public FakeValidateCreateProduct()
    {
        RuleFor(r => r.Name, _ => string.Empty);
        RuleFor(r => r.Barcode, _ => string.Empty);
        RuleFor(r => r.CategoryId, _ => Guid.Empty);
        RuleFor(r => r.ProfitMargin, _ => -1);
        RuleFor(r => r.Price, _ => -1);
    }
}
