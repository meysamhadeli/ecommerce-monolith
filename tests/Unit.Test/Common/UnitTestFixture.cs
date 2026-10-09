namespace Unit.Test.Common;

using AutoMapper;
using ECommerce.Data;

public class UnitTestFixture : IDisposable
{
    public UnitTestFixture()
    {
        Mapper = MapperFactory.Create();
        DbContext = DbContextFactory.Create();
    }

    public IMapper Mapper { get; }
    public ECommerceDbContext DbContext { get; }

    public void Dispose()
    {
        DbContextFactory.Destroy(DbContext);
    }
}
