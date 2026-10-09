namespace ECommerce.Extensions;

using Data;
using Data.Seed;
using FluentValidation;
using Griffin.EFCore;
using Griffin.Log;
using Griffin.ProblemDetails;
using Griffin.Validation;
using Griffin.Web;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Sieve.Services;

public static class InfrastructureExtensions
{
    public static WebApplicationBuilder AddInfrastructure(this WebApplicationBuilder builder)
    {
        builder.Services.AddProblemDetails();
        builder.Services.AddScoped<ISieveProcessor, SieveProcessor>();
        builder.Services.AddCustomMediatR(typeof(EcommerceRoot).Assembly);
        builder.Services.AddValidatorsFromAssembly(typeof(EcommerceRoot).Assembly);
        builder.Services.AddAutoMapper(typeof(EcommerceRoot).Assembly);
        builder.AddCustomDbContext<ECommerceDbContext>();
        builder.Services.AddScoped<IDataSeeder, ECommerceDataSeeder>();

        return builder;
    }

    public static WebApplication UseInfrastructure(this WebApplication app)
    {
        var appOptions = app.GetOptions<AppOptions>(nameof(AppOptions));

        app.UseCustomProblemDetails();

        app.UseMigration<ECommerceDbContext>();

        app.MapGet("/", x => x.Response.WriteAsync(appOptions.Name));

        return app;
    }
}
