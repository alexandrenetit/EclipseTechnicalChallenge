using Microsoft.Extensions.DependencyInjection;
using TaskManagement.Application.Interfaces;
using TaskManagement.Application.Services;

namespace TaskManagement.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationDependencyInjection(this IServiceCollection services)
        {
            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<IReportService, ReportService>();
            services.AddScoped<IWorkItemService, WorkItemService>();

            return services;
        }
    }
}