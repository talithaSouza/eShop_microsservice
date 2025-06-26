using GeekShopping.CartAPI.DTO;

namespace GeekShopping.CartAPI.Repository.Interfaces
{
    public interface ICouponRepository
    {
        Task<CouponDTO> GetCouponByCouponCode(string couponCode, string token); 
    }
}