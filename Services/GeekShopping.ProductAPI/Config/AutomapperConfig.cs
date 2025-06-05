using AutoMapper;
using GeekShopping.ProductAPI.DTO;
using GeekShopping.ProductAPI.Model;

namespace GeekShopping.ProductAPI.Config
{
    public static class AutomapperConfig
    {
        public static void RegisterMaps(this IServiceCollection services)
        {
            var autoMapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ProductDTO, Product>().ReverseMap();
            });

            services.AddSingleton(autoMapperConfig.CreateMapper());
        }
    }
}