namespace ECommerce.Data
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Design;

    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ECommerceDbContext>
    {
        public ECommerceDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<ECommerceDbContext>();

            builder.UseSqlServer(
                "Server=.\\sqlexpress;Database=EcommerceDB;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True");
            return new ECommerceDbContext(builder.Options);
        }
    }
}
