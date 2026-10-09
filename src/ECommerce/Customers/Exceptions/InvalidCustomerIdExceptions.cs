namespace ECommerce.Customers.Exceptions;

using Griffin.Core.Exception;

public class InvalidCustomerIdExceptions : BadRequestException
{
    public InvalidCustomerIdExceptions(Guid customerId)
        : base($"CustomerId: '{customerId}' is invalid.")
    {
    }
}
