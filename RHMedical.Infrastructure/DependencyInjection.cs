using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RHMedical.Infrastructure.Identity;
using RHMedical.Infrastructure.Persistence;
using RHMedical.Infrastructure.Users;
using Microsoft.EntityFrameworkCore;

namespace RHMedical.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<AzureB2BInviteService>();
            services.AddScoped<UserManagementService>();
            services.AddScoped<UserProvisioningService>();
            return services;
        }
    }
}