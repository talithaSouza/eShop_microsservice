using GeekShopping.CouponAPI.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GeekShopping.CouponAPI.Controller
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CouponController : ControllerBase
    {
         private ICouponRepository _repository;

        public CouponController(ICouponRepository repository)
        {
            _repository = repository ?? throw new
                ArgumentNullException(nameof(repository));
        }

        [HttpGet]
        public IActionResult Test() => Ok("Controller is working");

        [HttpGet("{couponCode}")]
        // [Authorize]
        public async Task<ActionResult> GetCouponByCouponCode(string couponCode)
        {
            var coupon = await _repository.GetCouponByCouponCode(couponCode);
            if (coupon == null)
                return NotFound();

            return Ok(coupon);
        }
        
    }
}