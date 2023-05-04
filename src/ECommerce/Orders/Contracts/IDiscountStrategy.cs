namespace ECommerce.Orders.Contracts;

public interface IDiscountStrategy
{
    decimal ApplyDiscount(decimal amount);
}
