namespace ECommerce.Extensions;

using BuildingBlocks.EFCore;
using BuildingBlocks.Logging;
using BuildingBlocks.Web;
using Data;
using Data.Seed;
using Figgle;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Sieve.Services;

public static class InfrastructureExtensions
{
    public static WebApplicationBuilder AddInfrastructure(this WebApplicationBuilder builder)
    {
        var env = builder.Environment;

        var appOptions = builder.Services.GetOptions<AppOptions>(nameof(AppOptions));
        Console.WriteLine(FiggleFonts.Standard.Render(appOptions.Name));

        builder.Services.AddProblemDetails();
        builder.AddCustomSerilog(env);
        builder.Services.AddScoped<ISieveProcessor, SieveProcessor>();
        builder.Services.AddCustomMediatR(typeof(EcommerceRoot).Assembly);
        builder.Services.AddValidatorsFromAssembly(typeof(EcommerceRoot).Assembly);
        builder.Services.AddAutoMapper(typeof(EcommerceRoot).Assembly);
        builder.Services.AddCustomDbContext<ECommerceDbContext>();
        builder.Services.AddScoped<IDataSeeder, ECommerceDataSeeder>();

        return builder;
    }


    public static WebApplication UseInfrastructure(this WebApplication app)
    {
        var env = app.Environment;
        var appOptions = app.GetOptions<AppOptions>(nameof(AppOptions));

        app.UseCustomProblemDetails();

        app.UseSerilogRequestLogging(options =>
        {
            options.EnrichDiagnosticContext = LogEnrichHelper.EnrichFromRequest;
        });

        app.UseMigration<ECommerceDbContext>(env);

        app.MapGet("/", x => x.Response.WriteAsync(appOptions.Name));

        return app;
    }
}
