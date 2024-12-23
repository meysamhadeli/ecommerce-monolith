using BuildingBlocks.Core.Event;
using BuildingBlocks.Core.Model;
using BuildingBlocks.Web;
using MassTransit;
using MassTransit.Testing;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Respawn;
using Serilog;
using Xunit;
using Xunit.Abstractions;
using ILogger = Serilog.ILogger;
using BuildingBlocks.EFCore;

namespace BuildingBlocks.TestBase;

using Microsoft.Data.SqlClient;
using Npgsql;
using Testcontainers.PostgreSql;

public class TestFixture<TEntryPoint> : IAsyncLifetime
    where TEntryPoint : class
{
    private readonly WebApplicationFactory<TEntryPoint> _factory;
    private static int Timeout => 120; // Second
    private ITestHarness TestHarness => ServiceProvider?.GetTestHarness();
    private Action<IServiceCollection> TestRegistrationServices { get; set; }
    private PostgreSqlContainer _postgreSqlContainer;

    public HttpClient HttpClient => _factory?.CreateClient();

    public IServiceProvider ServiceProvider => _factory?.Services;
    public IConfiguration Configuration => _factory?.Services.GetRequiredService<IConfiguration>();
    public ILogger Logger { get; set; }
    protected TestFixture()
    {
        _factory = new WebApplicationFactory<TEntryPoint>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureAppConfiguration(AddCustomAppSettings);

                builder.UseEnvironment("test");
                builder.ConfigureServices(services =>
                {
                    TestRegistrationServices?.Invoke(services);
                    services.ReplaceSingleton(AddHttpContextAccessorMock);

                    // Register all ITestDataSeeder implementations dynamically
                    services.Scan(scan => scan
                                      .FromApplicationDependencies() // Scan the current app and its dependencies
                                      .AddClasses(classes => classes.AssignableTo<ITestDataSeeder>()) // Find classes that implement ITestDataSeeder
                                      .AsImplementedInterfaces()
                                      .WithScopedLifetime());
                });
            });
    }

    public async Task InitializeAsync()
    {
        await StartTestContainerAsync();
    }

    public async Task DisposeAsync()
    {
        await StopTestContainerAsync();
        await _factory.DisposeAsync();
    }

    public virtual void RegisterServices(Action<IServiceCollection> services)
    {
        TestRegistrationServices = services;
    }

    // ref: https://github.com/trbenning/serilog-sinks-xunit
    public ILogger CreateLogger(ITestOutputHelper output)
    {
        if (output != null)
        {
            return new LoggerConfiguration()
                .WriteTo.TestOutput(output)
                .CreateLogger();
        }

        return null;
    }

    protected async Task ExecuteScopeAsync(Func<IServiceProvider, Task> action)
    {
        using var scope = ServiceProvider.CreateScope();
        await action(scope.ServiceProvider);
    }

    protected async Task<T> ExecuteScopeAsync<T>(Func<IServiceProvider, Task<T>> action)
    {
        using var scope = ServiceProvider.CreateScope();

        var result = await action(scope.ServiceProvider);

        return result;
    }

    public Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
    {
        return ExecuteScopeAsync(sp =>
        {
            var mediator = sp.GetRequiredService<IMediator>();

            return mediator.Send(request);
        });
    }

    public Task SendAsync(IRequest request)
    {
        return ExecuteScopeAsync(sp =>
        {
            var mediator = sp.GetRequiredService<IMediator>();
            return mediator.Send(request);
        });
    }

    public async Task Publish<TMessage>(TMessage message, CancellationToken cancellationToken = default)
        where TMessage : class, IEvent
    {
        await TestHarness.Bus.Publish(message, cancellationToken);
    }

    public async Task<bool> WaitForPublishing<TMessage>(CancellationToken cancellationToken = default)
        where TMessage : class, IEvent
    {
        var result = await WaitUntilConditionMet(async () =>
        {
            var published = await TestHarness.Published.Any<TMessage>(cancellationToken);
            var faulty = await TestHarness.Published.Any<Fault<TMessage>>(cancellationToken);

            return published && faulty == false;
        });
        return result;
    }

    public async Task<bool> WaitForConsuming<TMessage>(CancellationToken cancellationToken = default)
        where TMessage : class, IEvent
    {
        var result = await WaitUntilConditionMet(async () =>
        {
            var consumed = await TestHarness.Consumed.Any<TMessage>(cancellationToken);
            var faulty = await TestHarness.Consumed.Any<Fault<TMessage>>(cancellationToken);

            return consumed && faulty == false;
        });

        return result;
    }

    // Ref: https://tech.energyhelpline.com/in-memory-testing-with-masstransit/
    private static async Task<bool> WaitUntilConditionMet(Func<Task<bool>> conditionToMet, int? timeoutSecond = null)
    {
        var time = timeoutSecond ?? Timeout;

        var startTime = DateTime.Now;
        var timeoutExpired = false;
        var meet = await conditionToMet.Invoke();
        while (!meet)
        {
            if (timeoutExpired)
            {
                return false;
            }

            await Task.Delay(100);
            meet = await conditionToMet.Invoke();
            timeoutExpired = DateTime.Now - startTime > TimeSpan.FromSeconds(time);
        }

        return true;
    }


    private async Task StartTestContainerAsync()
    {
        _postgreSqlContainer = TestContainers.PostgresTestContainer();

        await _postgreSqlContainer.StartAsync();
    }

    private async Task StopTestContainerAsync()
    {
        await _postgreSqlContainer.StopAsync();
    }

    private void AddCustomAppSettings(IConfigurationBuilder configuration)
    {
        configuration.AddInMemoryCollection(new KeyValuePair<string, string>[]
        {
            new("PostgresOptions:ConnectionString", _postgreSqlContainer.GetConnectionString()),
        });
    }

    private IHttpContextAccessor AddHttpContextAccessorMock(IServiceProvider serviceProvider)
    {
        var httpContextAccessorMock = Substitute.For<IHttpContextAccessor>();
        using var scope = serviceProvider.CreateScope();
        httpContextAccessorMock.HttpContext = new DefaultHttpContext { RequestServices = scope.ServiceProvider };

        httpContextAccessorMock.HttpContext.Request.Host = new HostString("localhost", 6012);
        httpContextAccessorMock.HttpContext.Request.Scheme = "http";

        return httpContextAccessorMock;
    }
}

public class TestWriteFixture<TEntryPoint, TWContext> : TestFixture<TEntryPoint>
    where TEntryPoint : class
    where TWContext : DbContext
{
    public Task ExecuteDbContextAsync(Func<TWContext, Task> action)
    {
        return ExecuteScopeAsync(sp => action(sp.GetService<TWContext>()));
    }

    public Task ExecuteDbContextAsync(Func<TWContext, ValueTask> action)
    {
        return ExecuteScopeAsync(sp => action(sp.GetService<TWContext>()).AsTask());
    }

    public Task ExecuteDbContextAsync(Func<TWContext, IMediator, Task> action)
    {
        return ExecuteScopeAsync(sp => action(sp.GetService<TWContext>(), sp.GetService<IMediator>()));
    }

    public Task<T> ExecuteDbContextAsync<T>(Func<TWContext, Task<T>> action)
    {
        return ExecuteScopeAsync(sp => action(sp.GetService<TWContext>()));
    }

    public Task<T> ExecuteDbContextAsync<T>(Func<TWContext, ValueTask<T>> action)
    {
        return ExecuteScopeAsync(sp => action(sp.GetService<TWContext>()).AsTask());
    }

    public Task<T> ExecuteDbContextAsync<T>(Func<TWContext, IMediator, Task<T>> action)
    {
        return ExecuteScopeAsync(sp => action(sp.GetService<TWContext>(), sp.GetService<IMediator>()));
    }

    public Task InsertAsync<T>(params T[] entities) where T : class
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

    public async Task InsertAsync<TEntity>(TEntity entity) where TEntity : class
    {
        await ExecuteDbContextAsync(db =>
        {
            db.Set<TEntity>().Add(entity);

            return db.SaveChangesAsync();
        });
    }

    public Task InsertAsync<TEntity, TEntity2>(TEntity entity, TEntity2 entity2)
        where TEntity : class
        where TEntity2 : class
    {
        return ExecuteDbContextAsync(db =>
        {
            db.Set<TEntity>().Add(entity);
            db.Set<TEntity2>().Add(entity2);

            return db.SaveChangesAsync();
        });
    }

    public Task InsertAsync<TEntity, TEntity2, TEntity3>(TEntity entity, TEntity2 entity2, TEntity3 entity3)
        where TEntity : class
        where TEntity2 : class
        where TEntity3 : class
    {
        return ExecuteDbContextAsync(db =>
        {
            db.Set<TEntity>().Add(entity);
            db.Set<TEntity2>().Add(entity2);
            db.Set<TEntity3>().Add(entity3);

            return db.SaveChangesAsync();
        });
    }

    public Task InsertAsync<TEntity, TEntity2, TEntity3, TEntity4>(TEntity entity, TEntity2 entity2, TEntity3 entity3,
        TEntity4 entity4)
        where TEntity : class
        where TEntity2 : class
        where TEntity3 : class
        where TEntity4 : class
    {
        return ExecuteDbContextAsync(db =>
        {
            db.Set<TEntity>().Add(entity);
            db.Set<TEntity2>().Add(entity2);
            db.Set<TEntity3>().Add(entity3);
            db.Set<TEntity4>().Add(entity4);

            return db.SaveChangesAsync();
        });
    }

    public Task<T> FindAsync<T, TKey>(TKey id)
        where T : class, IEntity
    {
        return ExecuteDbContextAsync(db => db.Set<T>().FindAsync(id).AsTask());
    }

    public Task<T> FirstOrDefaultAsync<T>()
        where T : class, IEntity
    {
        return ExecuteDbContextAsync(db => db.Set<T>().FirstOrDefaultAsync());
    }
}

public class TestFixture<TEntryPoint, TWContext> : TestWriteFixture<TEntryPoint, TWContext>
    where TEntryPoint : class
    where TWContext : DbContext
{
}

public class TestFixtureCore<TEntryPoint> : IAsyncLifetime
    where TEntryPoint : class
{
    private Respawner _reSpawnerDefaultDb;
    private NpgsqlConnection _defaultDbConnection;

    public TestFixtureCore(TestFixture<TEntryPoint> integrationTestFixture, ITestOutputHelper outputHelper)
    {
        Fixture = integrationTestFixture;
        integrationTestFixture.RegisterServices(RegisterTestsServices);
        integrationTestFixture.Logger = integrationTestFixture.CreateLogger(outputHelper);
    }

    public TestFixture<TEntryPoint> Fixture { get; }


    public async Task InitializeAsync()
    {
        await InitDatabaseAsync();
    }

    public async Task DisposeAsync()
    {
        await ResetDatabaseAsync();
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

    protected virtual void RegisterTestsServices(IServiceCollection services)
    {
    }

    private async Task SeedDataAsync()
    {
        using var scope = Fixture.ServiceProvider.CreateScope();

        var seedManager = scope.ServiceProvider.GetService<ISeedManager>();
        await seedManager.ExecuteTestSeedAsync();
    }
}

public abstract class TestWriteBase<TEntryPoint, TWContext> : TestFixtureCore<TEntryPoint>
    where TEntryPoint : class
    where TWContext : DbContext
{
    protected TestWriteBase(
        TestWriteFixture<TEntryPoint, TWContext> integrationTestFixture, ITestOutputHelper outputHelper = null) : base(
        integrationTestFixture, outputHelper)
    {
        Fixture = integrationTestFixture;
    }

    public new TestWriteFixture<TEntryPoint, TWContext> Fixture { get; }
}

public abstract class TestBase<TEntryPoint, TWContext> : TestFixtureCore<TEntryPoint>
    where TEntryPoint : class
    where TWContext : DbContext
{
    protected TestBase(
        TestFixture<TEntryPoint, TWContext> integrationTestFixture, ITestOutputHelper outputHelper = null) :
        base(integrationTestFixture, outputHelper)
    {
        Fixture = integrationTestFixture;
    }

    public TestFixture<TEntryPoint, TWContext> Fixture { get; }
}
