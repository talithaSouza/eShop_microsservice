
using GeekShopping.CouponAPI.Repository;
using GeekShopping.CouponAPI.Repository.Interfaces;

namespace GeekShopping.CouponAPI.Config
{
    public static class DependencyInjection
    {
        public static IServiceCollection RegisterRepository(this IServiceCollection services)
        {
            #region P
            services.AddScoped<ICouponRepository, CouponRepository>();
            #endregion

            return services;
        }
    }
}