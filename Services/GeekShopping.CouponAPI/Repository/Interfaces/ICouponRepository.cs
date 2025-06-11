using GeekShopping.CouponAPI.DTO;

namespace GeekShopping.CouponAPI.Repository.Interfaces
{
    public interface ICouponRepository
    {
        Task<CouponDTO> GetCouponByCouponCode(string couponCode); 
    }
}