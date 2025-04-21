using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaskManagement.Domain.Repositories;
using TaskManagement.Infrastructure.Persistence;
using TaskManagement.Infrastructure.Repositories;

namespace TaskManagement.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureDependencyInjection(this IServiceCollection services,
        IConfiguration configuration)
    {
        string connectionStringTemplate = configuration.GetConnectionString("DefaultConnection")!;
        string connectionString = connectionStringTemplate
          .Replace("$MYSQL_HOST", Environment.GetEnvironmentVariable("MYSQL_HOST"))
          .Replace("$MYSQL_PASSWORD", Environment.GetEnvironmentVariable("MYSQL_PASSWORD"))
          .Replace("$MYSQL_DATABASE", Environment.GetEnvironmentVariable("MYSQL_DATABASE"))
          .Replace("$MYSQL_PORT", Environment.GetEnvironmentVariable("MYSQL_PORT"))
          .Replace("$MYSQL_USER", Environment.GetEnvironmentVariable("MYSQL_USER"));

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseMySQL(connectionString);
        });

        services.AddScoped<IProjectRepository, ProjectRepository>();
        services.AddScoped<IWorkItemRepository, WorkItemRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}