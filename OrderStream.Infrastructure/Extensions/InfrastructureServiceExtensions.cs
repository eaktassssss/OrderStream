using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderStream.Application.Interfaces.Repositories;
using OrderStream.Application.Services;
using OrderStream.Infrastructure.Implementations.Repositories;
using OrderStream.Infrastructure.Implementations.Services;

namespace OrderStream.Infrastructure.Extensions
{
    public static class InfrastructureServiceExtensions
    {
        public static IServiceCollection AddMongoDbRepositoriesAndServices(this IServiceCollection services, IConfiguration configuration)
        {
            var mongoDbSettings = new MongoDbSettings();
            configuration.GetSection("MongoDbSettings").Bind(mongoDbSettings);
            services.AddSingleton(mongoDbSettings);

            services.AddSingleton<MongoDbContext>();

            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();

            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IProductService, ProductService>();

            return services;
        }
    }
}