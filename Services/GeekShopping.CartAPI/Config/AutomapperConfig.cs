using AutoMapper;
using GeekShopping.CartAPI.Model;

namespace GeekShopping.CartAPI.Config
{
    public static class AutomapperConfig
    {
        public static void RegisterMaps(this IServiceCollection services)
        {
            var autoMapperConfig = new MapperConfiguration(cfg =>
            {
                // cfg.CreateMap<ProductDTO, Product>().ReverseMap();
            });

            services.AddSingleton(autoMapperConfig.CreateMapper());
        }
    }
}