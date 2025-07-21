using CleanSlice.Application.Abstractions.Data;
using Microsoft.Extensions.DependencyInjection;

namespace CleanSlice.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services)
    {

        services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();

        return services;
    }
}
