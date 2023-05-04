dotnet ef migrations add Init --context ECommerceDbContext -o "Data\Migrations"
dotnet ef database update --context ECommerceDbContext
