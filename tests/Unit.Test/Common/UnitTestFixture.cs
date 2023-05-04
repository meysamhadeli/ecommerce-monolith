namespace Unit.Test.Common
{
    using AutoMapper;
    using ECommerce.Data;
    using Xunit;

    [CollectionDefinition(nameof(UnitTestFixture))]
    public class FixtureCollection : ICollectionFixture<UnitTestFixture> { }

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
}
