namespace Integration.Test.Inventories.Features;

using BuildingBlocks.TestBase;
using ECommerce.Data;
using ECommerce.Inventories.Enums;
using ECommerce.Inventories.Models;
using ECommerce.Products.ValueObjects;
using Fakes;
using FluentAssertions;
using Xunit;

public class GetNumberOfProductsSeparatedTests : ECommerceIntegrationTestBase
{
    public GetNumberOfProductsSeparatedTests(
        TestFixture<ECommerce.Api.Program, ECommerceDbContext> integrationTestFactory) : base(integrationTestFactory)
    {
    }

    [Fact]
    public async Task should_retrive_nember_of_product_seprated_by_status_inventory_to_db()
    {
        //Arrange
        var query = new FakeGetNumberOfProductsSeparatedQuery().Generate();

        // Act
        var result = await Fixture.SendAsync(query);

        // Assert

        result.Should().NotBeNull();
        result.Select(x=> x.Status.Should().Be(ProductStatus.Damaged.ToString()));
        result.Select(x=> x.Status.Should().Be(ProductStatus.InStock.ToString()));
        result.Select(x=> x.Status.Should().Be(ProductStatus.Sold.ToString()));
        result.Count().Should().BeGreaterThanOrEqualTo(3);
    }
}
