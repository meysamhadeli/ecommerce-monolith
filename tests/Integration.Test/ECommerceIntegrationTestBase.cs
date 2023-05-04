namespace Integration.Test;

using BuildingBlocks.TestBase;
using ECommerce.Data;
using Xunit;


[Collection(IntegrationTestCollection.Name)]
public class ECommerceIntegrationTestBase: TestBase<ECommerce.Api.Program, ECommerceDbContext>
{
    public ECommerceIntegrationTestBase(TestFixture<ECommerce.Api.Program, ECommerceDbContext> integrationTestFixture) : base(integrationTestFixture)
    {
    }
}

[CollectionDefinition(Name)]
public class IntegrationTestCollection : ICollectionFixture<TestFixture<ECommerce.Api.Program, ECommerceDbContext>>
{
    public const string Name = "ECommerce Integration Test";
}
