using ECommerce;
using ECommerce.Extensions;
using Griffin.OpenApi;
using Griffin.Web;

var builder = WebApplication.CreateBuilder(args);

builder.AddMinimalEndpoints(assemblies: typeof(EcommerceRoot).Assembly);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCustomVersioning();
builder.Services.AddAspnetOpenApi();
builder.AddInfrastructure();

var app = builder.Build();

app.MapMinimalEndpoints();
app.UseInfrastructure();

if (!app.Environment.IsProduction())
{
    app.UseAspnetOpenApi();
}

app.Run();

namespace ECommerce.Api
{
    // For tests
    public partial class Program
    {
    }
}
