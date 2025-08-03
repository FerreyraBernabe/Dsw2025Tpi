using Dsw2025Tpi.Application.Interfaces;
using Dsw2025Tpi.Application.Services;
using Dsw2025Tpi.Data;
using Dsw2025Tpi.Data.Repositories;
using Dsw2025Tpi.Domain.Entities;
using Dsw2025Tpi.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Dsw2025Tpi.Api.Configurations
{
    public static class DomainServicesExtensions
    {
        public static IServiceCollection AddDomainServices(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddDbContext<Dsw2025TpiContext>(options =>

            {
                options.UseSqlServer(configuration.GetConnectionString("Dsw2025Tpi"));
            });

            services.AddScoped<IRepository, EfRepository>();
            services.AddScoped<ProductsManagementService>();
            services.AddScoped<OrdersManagementService>();
            services.AddScoped<IProductsManagementService, ProductsManagementService>();
            services.AddScoped<IOrdersManagementService, OrdersManagementService>();
            services.AddScoped<IAuthenticateService, AuthenticateService>();

            return services;
        }
    }
}
