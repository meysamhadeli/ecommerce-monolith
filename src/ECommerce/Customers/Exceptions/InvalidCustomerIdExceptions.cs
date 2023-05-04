namespace ECommerce.Customers.Exceptions;

using BuildingBlocks.Exception;

public class InvalidCustomerIdExceptions : BadRequestException
{
    public InvalidCustomerIdExceptions(Guid customerId)
        : base($"CustomerId: '{customerId}' is invalid.")
    {
    }
}
