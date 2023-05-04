namespace BuildingBlocks.TestBase;

using Testcontainers.MsSql;
using Web;

public static class TestContainers
{
    public static MsSqlContainerOptions MsSqlContainerConfiguration { get;}

    static TestContainers()
    {
        var configuration = ConfigurationHelper.GetConfiguration();

        MsSqlContainerConfiguration = configuration.GetOptions<MsSqlContainerOptions>(nameof(MsSqlContainerOptions));
    }

    public static MsSqlContainer MsSqlTestContainer()
    {
        var baseBuilder = new MsSqlBuilder()
            .WithPassword(MsSqlContainerConfiguration.Password)
            .WithLabel("Key", "Value");

        var builder = baseBuilder
            .WithImage(MsSqlContainerConfiguration.ImageName)
            .WithName(MsSqlContainerConfiguration.Name)
            .WithPortBinding(MsSqlContainerConfiguration.Port, true)
            .Build();

        return builder;
    }

    public sealed class MsSqlContainerOptions
    {
        public string Name { get; set; } = "msSql_" + Guid.NewGuid().ToString("D");
        public int Port { get; set; } = 1433;
        public string ImageName { get; set; } = "mcr.microsoft.com/mssql/server";
        public string UserName { get; set; } = Guid.NewGuid().ToString("D");
        public string Password { get; set; } = Guid.NewGuid().ToString("D");
    }
}
