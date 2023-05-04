namespace EndToEnd.Test;

using BuildingBlocks.TestBase;
using ECommerce.Data;
using Xunit;

[Collection(EndToEndTestCollection.Name)]
public class ECommerceEndToEndTestBase: TestBase<ECommerce.Api.Program, ECommerceDbContext>
{
    public ECommerceEndToEndTestBase(TestFixture<ECommerce.Api.Program, ECommerceDbContext> integrationTestFixture) : base(integrationTestFixture)
    {
    }
}

[CollectionDefinition(Name)]
public class EndToEndTestCollection : ICollectionFixture<TestFixture<ECommerce.Api.Program, ECommerceDbContext>>
{
    public const string Name = "ECommerce EndToEnd Test";
}

