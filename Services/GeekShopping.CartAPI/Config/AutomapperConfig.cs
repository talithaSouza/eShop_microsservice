using AutoMapper;
using GeekShopping.CartAPI.DTO;
using GeekShopping.CartAPI.Model;

namespace GeekShopping.CartAPI.Config
{
    public static class AutomapperConfig
    {
        public static void RegisterMaps(this IServiceCollection services)
        {
            var autoMapperConfig = new MapperConfiguration(cfg =>
            {
                //C
                cfg.CreateMap<CartHeaderDTO, CartHeader>().ReverseMap();

                cfg.CreateMap<CartDetailDTO, CartDetail>().ReverseMap();

                cfg.CreateMap<CartDTO, Cart>().ReverseMap();

                //P
                cfg.CreateMap<ProductDTO, Product>().ReverseMap();
            });

            services.AddSingleton(autoMapperConfig.CreateMapper());
        }
    }
}