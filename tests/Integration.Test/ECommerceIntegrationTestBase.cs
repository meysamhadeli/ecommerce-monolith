namespace Integration.Test;

using ECommerce.TestBase;
using ECommerce.Data;
using Griffin.EFCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

[Collection(IntegrationTestCollection.Name)]
public class ECommerceIntegrationTestBase: TestBase<ECommerce.Api.Program, ECommerceDbContext>
{
    public ECommerceIntegrationTestBase(TestFixture<ECommerce.Api.Program, ECommerceDbContext> integrationTestFixture) : base(integrationTestFixture)
    {
    }

    protected override void RegisterTestsServices(IServiceCollection services)
    {
        services.AddScoped<ITestDataSeeder, ECommerceTestDataSeeder>();
    }
}

[CollectionDefinition(Name)]
public class IntegrationTestCollection : ICollectionFixture<TestFixture<ECommerce.Api.Program, ECommerceDbContext>>
{
    public const string Name = "ECommerce Integration Test";
}
