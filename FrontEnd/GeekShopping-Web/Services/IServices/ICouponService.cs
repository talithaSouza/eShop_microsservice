using GeekShopping_Web.Models;

namespace GeekShopping_Web.Services.IServices
{
    public interface ICouponService
    {
        Task<CouponViewModel> GetCoupon(string code, string token);
    }
}