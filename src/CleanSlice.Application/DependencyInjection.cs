using CleanSlice.Application.Abstractions.Behaviors;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace CleanSlice.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);

            // 1. Logging & Exception Handling
            config.AddOpenBehavior(typeof(ExceptionHandlingBehavior<,>));
            config.AddOpenBehavior(typeof(LoggingBehavior<,>));

            // 2. Validation
            config.AddOpenBehavior(typeof(ValidationBehavior<,>));

            // 3. Caching
            // config.AddOpenBehavior(typeof(CachingBehavior<,>));

            // 4. Transaction
            config.AddOpenBehavior(typeof(TransactionalBehavior<,>));
        });

        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly, includeInternalTypes: true);

        // AutoMapper
        services.AddAutoMapper(typeof(DependencyInjection).Assembly);

        return services;
    }
}
