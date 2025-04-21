using Microsoft.Extensions.DependencyInjection;
using TaskManagement.Domain.Services;

namespace TaskManagement.Domain;

public static class DependencyInjection
{
    public static IServiceCollection AddDomainDependencyInjection(this IServiceCollection services)
    {
        services.AddScoped<IWorkItemServiceDomain, WorkItemServiceDomain>();

        return services;
    }
}