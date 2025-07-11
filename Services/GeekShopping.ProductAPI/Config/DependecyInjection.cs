using GeekShopping.ProductAPI.Repository;
using GeekShopping.ProductAPI.Repository.Interfaces;

namespace GeekShopping.ProductAPI.Config
{
    public static class DependencyInjection
    {
        public static IServiceCollection RegisterRepository(this IServiceCollection services)
        {
            #region P
            services.AddScoped<IProductRepository, ProductRepository>();
            #endregion

            return services;
        }
    }
}