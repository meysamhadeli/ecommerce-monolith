namespace ECommerce.Orders.Contracts.Strategies.Discount;

public class PercentageDiscountStrategy : IDiscountStrategy
{
    public decimal Percentage { get; set; }

    public PercentageDiscountStrategy(decimal percentage)
    {
        Percentage = percentage;
    }

    public decimal ApplyDiscount(decimal amount)
    {
        return amount * Percentage / 100;
    }
}
