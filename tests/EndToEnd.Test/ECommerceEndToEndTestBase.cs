namespace EndToEnd.Test;

using ECommerce.TestBase;
using ECommerce.Data;
using Griffin.EFCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

[Collection(EndToEndTestCollection.Name)]
public class ECommerceEndToEndTestBase: TestBase<ECommerce.Api.Program, ECommerceDbContext>
{
    public ECommerceEndToEndTestBase(TestFixture<ECommerce.Api.Program, ECommerceDbContext> integrationTestFixture) : base(integrationTestFixture)
    {
    }

    protected override void RegisterTestsServices(IServiceCollection services)
    {
        services.AddScoped<ITestDataSeeder, ECommerceTestDataSeeder>();
    }
}

[CollectionDefinition(Name)]
public class EndToEndTestCollection : ICollectionFixture<TestFixture<ECommerce.Api.Program, ECommerceDbContext>>
{
    public const string Name = "ECommerce EndToEnd Test";
}

