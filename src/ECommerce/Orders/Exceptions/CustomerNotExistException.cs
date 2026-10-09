namespace ECommerce.Orders.Exceptions;

using Griffin.Core.Exception;

public class CustomerNotExistException: NotFoundException
{
    public CustomerNotExistException(int? code = null) : base("Customer Not Exist!", code)
    {
    }
}
