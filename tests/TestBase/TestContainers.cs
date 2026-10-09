namespace ECommerce.TestBase;

using Testcontainers.PostgreSql;

public static class TestContainers
{
    public static PostgreSqlContainer PostgresTestContainer()
    {
        return new PostgreSqlBuilder()
            .WithImage("postgres:17")
            .WithName("postgres_" + Guid.NewGuid().ToString("D"))
            .WithUsername("postgres")
            .WithPassword("postgres")
            .WithCommand("-c", "max_prepared_transactions=10")
            .WithPortBinding(5432, true)
            .Build();
    }
}
