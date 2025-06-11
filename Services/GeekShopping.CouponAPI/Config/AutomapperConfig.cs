using AutoMapper;
using GeekShopping.CouponAPI.DTO;
using GeekShopping.CouponAPI.Model;

namespace GeekShopping.CouponAPI.Config
{
    public static class AutomapperConfig
    {
        public static void RegisterMaps(this IServiceCollection services)
        {
            var autoMapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CouponDTO, Coupon>().ReverseMap();
            });

            services.AddSingleton(autoMapperConfig.CreateMapper());
        }
    }
}