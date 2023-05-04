namespace ECommerce.Orders.Exceptions;

using BuildingBlocks.Exception;

public class CustomerNotExistException: NotFoundException
{
    public CustomerNotExistException(int? code = null) : base("Customer Not Exist!", code)
    {
    }
}
