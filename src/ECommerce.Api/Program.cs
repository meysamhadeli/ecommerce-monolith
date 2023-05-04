using BuildingBlocks.Swagger;
using BuildingBlocks.Web;
using ECommerce;
using ECommerce.Extensions;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCustomSwagger(configuration, typeof(EcommerceRoot).Assembly);
builder.Services.AddCustomVersioning();
builder.AddMinimalEndpoints(assemblies: typeof(EcommerceRoot).Assembly);
builder.AddInfrastructure();

var app = builder.Build();

app.MapMinimalEndpoints();
app.UseInfrastructure();

if (!app.Environment.IsProduction())
{
    app.UseCustomSwagger();
}

app.Run();

namespace ECommerce.Api
{
    // For tests
    public partial class Program
    {
    }
}
