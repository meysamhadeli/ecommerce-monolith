namespace EndToEnd.Test.Routes;

public static class ApiRoutes
{
    private const string BaseApiPath = "api/v1.0";

    public static class Catalog
    {
        public const string CreateCategory = $"{BaseApiPath}/catalog/category";
        public const string CreateProduct = $"{BaseApiPath}/catalog/product";
    }

    public static class Inventory
    {
        public const string AddProductToInventory = $"{BaseApiPath}/inventory/add-product-to-inventory";
        public const string ChangeProductStatus = $"{BaseApiPath}/inventory/change-product-status";
        public const string DamageProduct = $"{BaseApiPath}/inventory/damage-product";
        public const string SellProduct = $"{BaseApiPath}/inventory/sell-product";
        public const string GetNumberOfProductsSeparated = $"{BaseApiPath}/inventory/get-number-of-products-separated";
    }
}

