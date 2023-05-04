namespace ECommerce.Customers.Models;

using BuildingBlocks.Core.Event;
using BuildingBlocks.Core.Model;
using Inventories.ValueObjects;
using ValueObjects;
using Name = ValueObjects.Name;

public record Customer : Aggregate<CustomerId>
{
    public Name Name { get; private set; }
    public Mobile Mobile { get; private set; }
    public Address Address { get; private set; }

    public static Customer Create(CustomerId id, Name name, Mobile mobile, Address address, bool isDeleted = false)
    {
        var customer = new Customer
        {
            Id = id,
            Name = name,
            Mobile = mobile,
            Address = address,
            IsDeleted = isDeleted
        };

        var @event = new CustomerCreatedDomainEvent(customer.Id.Value, customer.Name.Value, customer.Mobile.Value,
            customer.Address.Value, customer.IsDeleted);

        customer.AddDomainEvent(@event);

        return customer;
    }
}

public record CustomerCreatedDomainEvent
    (Guid Id, string Name, string Mobile, string Address, bool IsDeleted) : IDomainEvent;
