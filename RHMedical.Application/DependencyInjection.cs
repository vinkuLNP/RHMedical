using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RHMedical.Application.Identity;
using RHMedical.Application.Users;
using RHMedical.Data.Persistence;

namespace RHMedical.Application
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
