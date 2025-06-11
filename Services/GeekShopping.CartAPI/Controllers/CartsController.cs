
using GeekShopping.CartAPI.DTO;
using GeekShopping.CartAPI.Messages;
using GeekShopping.CartAPI.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeekShopping.CartAPI.Controllers
{

    [Route("api/v1/[controller]")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly ICartRepository _repository;

        public CartsController(ICartRepository repository)
        {
            _repository = repository;
        }


        [HttpGet("find-cart/{userId}")]
        public async Task<ActionResult> FindById(string userId)
        {
            var cart = await _repository.FindCartByUserId(userId);

            if (cart == null)
                return NotFound();

            return Ok(cart);
        }

        [HttpPost("add-cart")]
        public async Task<ActionResult> AddCart(CartDTO DTO)
        {
            var cart = await _repository.SaveOrUpdateCart(DTO);

            if (cart == null)
                return NotFound();
            return Ok(cart);
        }



        [HttpPut("update-cart")]
        public async Task<ActionResult> UpdateCart(CartDTO DTO)
        {
            var cart = await _repository.SaveOrUpdateCart(DTO);

            if (cart == null)
                return NotFound();
            return Ok(cart);
        }

        [HttpDelete("remove-cart/{id}")]
        public async Task<ActionResult> RemoveCart(int id)
        {
            var status = await _repository.RemoveFromCart(id);

            if (!status)
                return BadRequest();

            return Ok(status);
        }

        [HttpPost("apply-coupon")]
        public async Task<ActionResult> ApplyCoupon(CartDTO vo)
        {
            var status = await _repository.ApplyCoupon(vo.CartHeader.UserId, vo.CartHeader.CouponCode);

            if (!status)
                return NotFound();

            return Ok(status);
        }

        [HttpDelete("remove-coupon/{userId}")]
        public async Task<ActionResult> ApplyCoupon(string userId)
        {
            var status = await _repository.RemoveCoupon(userId);

            if (!status)
                return NotFound();

            return Ok(status);
        }


        [HttpPost("checkout")]
        public async Task<ActionResult> Checkout(CheckoutHeader checkoutHeader)
        {
            var cart = await _repository.FindCartByUserId(checkoutHeader.UserId);

            if (cart == null)
                return NotFound();

            checkoutHeader.CartDetails = cart.CartDetails;

            //RabbitMQ
            
            return Ok(cart);
        }
    }
}
