using ApiTemplate.Application.Interfaces;
using ApiTemplate.Application.Services;
using ApiTemplate.Domain.Interfaces;
using ApiTemplate.Infrastructure.Data;
using ApiTemplate.Infrastructure.Repositories;
using ApiTemplate.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ApiTemplate.Infrastructure.Extensions
{
    public static class DataExtensions
    {
        public static IServiceCollection AddApiContext(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });      

            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {

            // Repositories
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
           
            // Services            
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ITokenService, TokenService>();

            return services;
        }
    }
}
