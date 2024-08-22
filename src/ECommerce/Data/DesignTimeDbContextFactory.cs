namespace ECommerce.Data
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Design;

    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ECommerceDbContext>
    {
        public ECommerceDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<ECommerceDbContext>();

            builder.UseNpgsql("Server=localhost;Port=5432;Database=ecommerce_db;User Id=postgres;Password=postgres;Include Error Detail=true");
            return new ECommerceDbContext(builder.Options);
        }
    }
}
