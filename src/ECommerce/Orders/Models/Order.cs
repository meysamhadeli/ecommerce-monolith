namespace ECommerce.Orders.Models;

using BuildingBlocks.Core.Model;
using Contracts;
using Customers.Models;
using Customers.ValueObjects;
using Dtos;
using Enums;
using Exceptions;
using Features.RegisteringNewOrder;
using ValueObjects;

public record Order : Aggregate<OrderId>
{
    private readonly List<OrderItem> _orderItems = new List<OrderItem>();
    public CustomerId CustomerId { get; private set; }
    public Customer Customer { get; private set; }
    public OrderStatus Status { get; private set; }
    public TotalPrice TotalPrice { get; private set; }
    public OrderDate OrderDate { get; private set; }


    public static Order Create(OrderId id, Customer customer, DiscountType discountType,
        decimal discountValue,
        bool isDeleted = false)
    {
        var order = new Order
        {
            Id = id,
            Customer = customer,
            CustomerId = customer.Id,
            TotalPrice = TotalPrice.Of(0),
            Status = OrderStatus.Pending,
            OrderDate = OrderDate.Of(DateTime.Now),
            IsDeleted = isDeleted
        };

        var @event = new OrderInitialedDomainEvent(order.Id, order.CustomerId, discountType, discountValue,
            order.Status, order.IsDeleted);

        order.AddDomainEvent(@event);

        return order;
    }


    public void AddItems(IEnumerable<OrderItem> items)
    {
        _orderItems.AddRange(items);
    }

    public void ApplyDiscount(DiscountType discountType, decimal discountValue)
    {
        var discountStrategy = DiscountStrategyFactory.CreateDiscountStrategy(discountType, discountValue);

        if (discountStrategy != null)
        {
            TotalPrice.Value -= discountStrategy.ApplyDiscount(TotalPrice.Value);

            var @event = new OrderDiscountAppliedDomainEvent(this.Id, this.CustomerId, discountType, discountValue,
                this.Status, this.IsDeleted);

            this.AddDomainEvent(@event);
        }
    }

    public (IEnumerable<OrderItemDto> ExpressShipmentItems, IEnumerable<OrderItemDto> RegularShipmentItems) ApplyShipment()
    {
        var regularItems = new List<OrderItem>();
        var expressItems = new List<OrderItem>();

        var shipmentRegularPostStrategy = ShipmentStrategyFactory.CreateShipmentStrategy(ShipmentType.RegularPost);

        if (shipmentRegularPostStrategy != null)
        {
            regularItems = shipmentRegularPostStrategy.GetOrderItemsToShip(_orderItems);
            if (regularItems.Any())
            {
                TotalPrice.Value += shipmentRegularPostStrategy.GetShipmentPriceُ();
            }
        }

        var shipmentExpressPostStrategy = ShipmentStrategyFactory.CreateShipmentStrategy(ShipmentType.ExpressPost);

        if (shipmentExpressPostStrategy != null)
        {
            expressItems = shipmentExpressPostStrategy.GetOrderItemsToShip(_orderItems);
            if (expressItems.Any())
            {
                TotalPrice.Value += shipmentExpressPostStrategy.GetShipmentPriceُ();
            }
        }

        if (!regularItems.Any() && !expressItems.Any())
        {
            throw new InvalidOrderQuantityException();
        }

        var regularItemsDto = regularItems?.Select(x => new OrderItemDto(x.Id, x.ProductId, x.OrderId, x.Quantity))
            .ToList();
        var expressItemsDto = expressItems?.Select(x => new OrderItemDto(x.Id, x.ProductId, x.OrderId, x.Quantity))
            .ToList();

        var @event = new OrderShipmentAppliedDomainEvent(this.Id, this.CustomerId, regularItemsDto, expressItemsDto,
            this.Status, this.IsDeleted);

        this.AddDomainEvent(@event);

        return (expressItemsDto, regularItemsDto);
    }

    public void CalculateTotalPrice()
    {
        TotalPrice.Value = _orderItems.Sum(item => item.CalculatePrice());

        if (TotalPrice.Value < 50000)
        {
            throw new InvalidTotalPriceRangeException(TotalPrice.Value);
        }

        var @event = new OrderTotalPriceAddedDomainEvent(this.Id.Value, this.CustomerId.Value, this.OrderDate,
            this.TotalPrice.Value,
            this.Status, _orderItems?.Select(x => new OrderItemDto(x.Id, x.ProductId, x.OrderId, x.Quantity)),
            this.IsDeleted);

        this.AddDomainEvent(@event);
    }
}
