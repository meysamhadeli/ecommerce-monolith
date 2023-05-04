namespace ECommerce.Products.Exceptions;

using BuildingBlocks.Exception;

public class InvalidProfitMarginException : BadRequestException
{
    public InvalidProfitMarginException(decimal profitMargin)
        : base($"ProfitMargin: '{profitMargin}' must be equal or grater than 0.")
    {
    }
}

