namespace ECommerce.Orders.Contracts.Strategies.Discount;

public class AmountDiscountStrategy : IDiscountStrategy
{
    public decimal Amount { get; set; }

    public AmountDiscountStrategy(decimal amount)
    {
        Amount = amount;
    }

    public decimal ApplyDiscount(decimal amount)
    {
        if (amount >= Amount)
        {
            return Amount;
        }

        return 0;
    }
}
