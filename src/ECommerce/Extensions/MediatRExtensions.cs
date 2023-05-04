namespace ECommerce.Extensions;

using System.Reflection;
using BuildingBlocks.EFCore;
using BuildingBlocks.Logging;
using BuildingBlocks.Validation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

public static class MediatRExtensions
{
    public static IServiceCollection AddCustomMediatR(this IServiceCollection services, params Assembly[] assemblies)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assemblies));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(EfTxBehavior<,>));

        return services;
    }
}
