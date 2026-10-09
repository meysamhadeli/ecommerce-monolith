dotnet ef migrations add Init --context ECommerceDbContext -o "Migrations" --project src/ECommerce --startup-project src/ECommerce.Api
dotnet ef database update --context ECommerceDbContext --project src/ECommerce --startup-project src/ECommerce.Api
