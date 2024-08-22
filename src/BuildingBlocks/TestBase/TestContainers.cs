namespace BuildingBlocks.TestBase;

using Testcontainers.PostgreSql;
using Web;

public static class TestContainers
{
    public static PostgresContainerOptions PostgresContainerConfiguration { get; }

    static TestContainers()
    {
        var configuration = ConfigurationHelper.GetConfiguration();

        PostgresContainerConfiguration =
            configuration.GetOptions<PostgresContainerOptions>(nameof(PostgresContainerOptions));
    }

    public static PostgreSqlContainer PostgresTestContainer()
    {
        var baseBuilder = new PostgreSqlBuilder()
            .WithUsername(PostgresContainerConfiguration.UserName)
            .WithPassword(PostgresContainerConfiguration.Password)
            .WithLabel("Key", "Value");

        var builder = baseBuilder
            .WithImage(PostgresContainerConfiguration.ImageName)
            .WithName(PostgresContainerConfiguration.Name)
            .WithCommand(new string[2] { "-c", "max_prepared_transactions=10" })
            .WithPortBinding(PostgresContainerConfiguration.Port, true)
            .Build();

        return builder;
    }

    public sealed class PostgresContainerOptions
    {
        public string Name { get; set; } = "postgreSql_" + Guid.NewGuid().ToString("D");
        public int Port { get; set; } = 5432;
        public string ImageName { get; set; } = "postgres:latest";
        public string UserName { get; set; } = Guid.NewGuid().ToString("D");
        public string Password { get; set; } = Guid.NewGuid().ToString("D");
    }
}
