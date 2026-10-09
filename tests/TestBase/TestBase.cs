namespace ECommerce.TestBase;

using Griffin.EFCore;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Respawn;
using Testcontainers.PostgreSql;
using Xunit;
using Xunit.Abstractions;

public class TestFixture<TEntryPoint> : IAsyncLifetime
    where TEntryPoint : class
{
    private readonly WebApplicationFactory<TEntryPoint> _factory;
    private readonly PostgreSqlContainer _postgreSqlContainer;
    private Action<IServiceCollection> _testRegistrationServices;

    public TestFixture()
    {
        _postgreSqlContainer = TestContainers.PostgresTestContainer();

        _factory = new WebApplicationFactory<TEntryPoint>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureAppConfiguration(AddCustomAppSettings);
                builder.UseEnvironment("test");
                builder.ConfigureServices(services =>
                {
                    _testRegistrationServices?.Invoke(services);

                    // Register all ITestDataSeeder implementations dynamically.
                    services.Scan(scan => scan
                        .FromApplicationDependencies()
                        .FromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
                        .AddClasses(classes => classes.AssignableTo<ITestDataSeeder>())
                        .AsImplementedInterfaces()
                        .WithScopedLifetime());
                });
            });
    }

    public HttpClient HttpClient => _factory.CreateClient();

    public IServiceProvider ServiceProvider => _factory.Services;

    public IConfiguration Configuration => _factory.Services.GetRequiredService<IConfiguration>();

    public async Task InitializeAsync()
    {
        await _postgreSqlContainer.StartAsync();
    }

    public async Task DisposeAsync()
    {
        await _postgreSqlContainer.StopAsync();
        await _factory.DisposeAsync();
    }

    public virtual void RegisterServices(Action<IServiceCollection> services)
    {
        _testRegistrationServices += services;
    }

    protected async Task ExecuteScopeAsync(Func<IServiceProvider, Task> action)
    {
        using var scope = ServiceProvider.CreateScope();
        await action(scope.ServiceProvider);
    }

    protected async Task<T> ExecuteScopeAsync<T>(Func<IServiceProvider, Task<T>> action)
    {
        using var scope = ServiceProvider.CreateScope();

        return await action(scope.ServiceProvider);
    }

    public Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
    {
        return ExecuteScopeAsync(sp => sp.GetRequiredService<IMediator>().Send(request));
    }

    public Task SendAsync(IRequest request)
    {
        return ExecuteScopeAsync(sp => sp.GetRequiredService<IMediator>().Send(request));
    }

    private void AddCustomAppSettings(IConfigurationBuilder configuration)
    {
        configuration.AddInMemoryCollection(new KeyValuePair<string, string>[]
        {
            new("PostgresOptions:ConnectionString", _postgreSqlContainer.GetConnectionString()),
        });
    }
}

public class TestWriteFixture<TEntryPoint, TWContext> : TestFixture<TEntryPoint>
    where TEntryPoint : class
    where TWContext : DbContext
{
    public Task ExecuteDbContextAsync(Func<TWContext, Task> action)
    {
        return ExecuteScopeAsync(sp => action(sp.GetRequiredService<TWContext>()));
    }

    public Task ExecuteDbContextAsync(Func<TWContext, IMediator, Task> action)
    {
        return ExecuteScopeAsync(sp => action(sp.GetRequiredService<TWContext>(), sp.GetRequiredService<IMediator>()));
    }

    public Task<T> ExecuteDbContextAsync<T>(Func<TWContext, Task<T>> action)
    {
        return ExecuteScopeAsync(sp => action(sp.GetRequiredService<TWContext>()));
    }

    public Task<T> ExecuteDbContextAsync<T>(Func<TWContext, IMediator, Task<T>> action)
    {
        return ExecuteScopeAsync(sp => action(sp.GetRequiredService<TWContext>(), sp.GetRequiredService<IMediator>()));
    }

    public Task InsertAsync<T>(params T[] entities)
        where T : class
    {
        return ExecuteDbContextAsync(db =>
        {
            foreach (var entity in entities)
            {
                db.Set<T>().Add(entity);
            }

            return db.SaveChangesAsync();
        });
    }
}

public class TestFixture<TEntryPoint, TWContext> : TestWriteFixture<TEntryPoint, TWContext>
    where TEntryPoint : class
    where TWContext : DbContext
{
}

public class TestFixtureCore<TEntryPoint, TWContext> : IAsyncLifetime
    where TEntryPoint : class
    where TWContext : DbContext
{
    private Respawner _reSpawnerDefaultDb;
    private NpgsqlConnection _defaultDbConnection;

    public TestFixtureCore(TestFixture<TEntryPoint, TWContext> integrationTestFixture, ITestOutputHelper outputHelper)
    {
        Fixture = integrationTestFixture;
        integrationTestFixture.RegisterServices(RegisterTestsServices);
    }

    public TestFixture<TEntryPoint, TWContext> Fixture { get; }

    public async Task InitializeAsync()
    {
        await InitDatabaseAsync();
    }

    public async Task DisposeAsync()
    {
        await ResetDatabaseAsync();
    }

    protected virtual void RegisterTestsServices(IServiceCollection services)
    {
    }

    private async Task InitDatabaseAsync()
    {
        var postgresOptions = Fixture.ServiceProvider.GetService<PostgresOptions>();

        if (!string.IsNullOrEmpty(postgresOptions?.ConnectionString))
        {
            _defaultDbConnection = new NpgsqlConnection(postgresOptions.ConnectionString);
            await _defaultDbConnection.OpenAsync();

            _reSpawnerDefaultDb = await Respawner.CreateAsync(_defaultDbConnection,
                new RespawnerOptions { DbAdapter = DbAdapter.Postgres });

            await SeedDataAsync();
        }
    }

    private async Task ResetDatabaseAsync()
    {
        if (_defaultDbConnection is not null)
        {
            await _reSpawnerDefaultDb.ResetAsync(_defaultDbConnection);
        }
    }

    private async Task SeedDataAsync()
    {
        using var scope = Fixture.ServiceProvider.CreateScope();

        var seedManager = scope.ServiceProvider.GetRequiredService<ISeedManager>();
        await seedManager.ExecuteTestSeedAsync();
    }
}

public abstract class TestBase<TEntryPoint, TWContext> : TestFixtureCore<TEntryPoint, TWContext>
    where TEntryPoint : class
    where TWContext : DbContext
{
    protected TestBase(TestFixture<TEntryPoint, TWContext> integrationTestFixture)
        : base(integrationTestFixture, null)
    {
    }
}
