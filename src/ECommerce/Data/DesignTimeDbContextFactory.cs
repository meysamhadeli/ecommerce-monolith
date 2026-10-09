namespace ECommerce.Data;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ECommerceDbContext>
{
    public ECommerceDbContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<ECommerceDbContext>();

        // Mirror the runtime configuration used by Griffin's AddCustomDbContext so the
        // migrations generated here describe the model the application actually uses
        // (snake_case table/column names and legacy timestamp behaviour).
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        builder.UseNpgsql(
                "Server=localhost;Port=5432;Database=ecommerce_db;User Id=postgres;Password=postgres;Include Error Detail=true")
            .UseSnakeCaseNamingConvention();

        return new ECommerceDbContext(builder.Options);
    }
}
