namespace Unit.Test.Fakes;

using ECommerce.Categories.ValueObjects;
using ECommerce.Customers.Models;
using ECommerce.Customers.ValueObjects;
using ECommerce.Orders.Enums;
using ECommerce.Orders.Models;
using ECommerce.Orders.ValueObjects;
using ECommerce.Products.Models;
using ECommerce.Products.ValueObjects;
using MassTransit;
using CustomerName = ECommerce.Customers.ValueObjects.Name;
using ProductName = ECommerce.Products.ValueObjects.Name;

public sealed class FakeOrder
{
    public static Order Generate()
    {
        var product = Product.Create(ProductId.Of(new Guid("3c5c0000-97c6-fc34-fcd3-08db322230c0")),
            ProductName.Of("Cake"), Barcode.Of("1234567890"), true,
            CategoryId.Of(new Guid("3c5c0000-97c6-fc34-fc3c-08db322230c8")), Price.Of(50000), ProfitMargin.Of(0),
            Description.Of("It's a Cake"));

        var orderId = OrderId.Of(NewId.NextGuid());

        var order = Order.Create(OrderId.Of(orderId),
            Customer.Create(CustomerId.Of(new Guid("2c5c0000-97c6-fc34-fcd3-08db322230c0")),
                CustomerName.Of("Admin"), Mobile.Of("09360000000"),
                Address.Of("Tehran", "Tehran", "Rey"))
            , DiscountType.AmountDiscount, 1800, OrderDate.Of(new DateTime(2023, 1, 1, 18, 0, 0)));


        order.AddItems(new List<OrderItem>(new[]
        {
            new OrderItem
            {
                Id = OrderItemId.Of(NewId.NextGuid()),
                OrderId = OrderId.Of(orderId),
                Product = product,
                ProductId = product.Id,
                Quantity = Quantity.Of(2)
            }
        }));

        return order;
    }
}
