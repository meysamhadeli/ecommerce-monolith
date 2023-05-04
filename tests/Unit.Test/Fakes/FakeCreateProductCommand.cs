namespace Unit.Test.Fakes;

using AutoBogus;
using ECommerce.Products.Features.CreatingProduct;
using MassTransit;

public sealed class FakeCreateProductCommand : AutoFaker<CreateProduct>
{
    public FakeCreateProductCommand()
    {
        RuleFor(r => r.Id, _ => NewId.NextGuid());
        RuleFor(r => r.Name, r => "Coffee");
        RuleFor(r => r.Description, r => "This is a coffee");
        RuleFor(r => r.CategoryId, r => new Guid("3c5c0000-97c6-fc34-fc3c-08db322230c8"));
        RuleFor(r => r.Barcode, r => "12928282");
        RuleFor(r => r.Weighted, r => true);
    }
}

