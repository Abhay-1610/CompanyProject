using CompanyProject.Application.History;
using CompanyProject.Application.Interfaces;
using CompanyProject.Infrastructure.Repositories;
using CompanyProject.Infrastructure.Security;
using CompanyProject.Infrastructure.SignalR;
using Microsoft.Extensions.DependencyInjection;

namespace CompanyProject.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            // Repositories
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IChangeHistoryRepository, ChangeHistoryRepository>();
            services.AddScoped<IRealtimeNotifier, SignalRNotifier>();
            services.AddScoped<ICurrentUser, CurrentUserService>();
            services.AddHttpContextAccessor();

            return services;
        }
    }
}
