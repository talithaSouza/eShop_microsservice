
using GeekShopping.CartAPI.DTO;
using GeekShopping.CartAPI.Messages;
using GeekShopping.CartAPI.RabbitMQSender;
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
        private readonly ICouponRepository _couponRepository;
        private readonly IRabbitMQMessageSender _rabbitMQMessageSender;

        public CartsController(ICartRepository repository, IRabbitMQMessageSender rabbitMQMessageSender, ICouponRepository couponRepository)
        {
            _repository = repository;
            _rabbitMQMessageSender = rabbitMQMessageSender;
            _couponRepository = couponRepository;
        }


        [HttpGet("find-cart/{userId}")]
        public async Task<ActionResult> FindById(string userId)
        {
            var cart = await _repository.FindCartByUserId(userId);

            if (cart == null)
                return NotFound();

            return Ok(cart);
        }

        [Authorize]
        [HttpPost("add-cart")]
        public async Task<ActionResult> AddCart([FromBody] CartDTO DTO)
        {
            var cart = await _repository.SaveOrUpdateCart(DTO);

            System.Console.WriteLine("Passou na Carts Controller Back");

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
            if (!status) return NotFound();
            return Ok(status);
        }

        [HttpDelete("remove-coupon/{userId}")]
        public async Task<ActionResult> RemoveCoupon(string userId)
        {
            var status = await _repository.RemoveCoupon(userId);
            if (!status) return NotFound();
            return Ok(status);
        }

        [HttpPost("checkout")]
        public async Task<ActionResult> Checkout(CheckoutHeaderDTO checkoutHeaderDTO)
        {
            try
            {

                if (checkoutHeaderDTO?.UserId == null)
                    return BadRequest();

                var cart = await _repository.FindCartByUserId(checkoutHeaderDTO.UserId);

                if (cart == null)
                    return NotFound();

                await VerifyCoupon(checkoutHeaderDTO);

                checkoutHeaderDTO.CartDetails = cart.CartDetails;
                checkoutHeaderDTO.DateTime = DateTime.Now;

                await _rabbitMQMessageSender.SendMessageAsync(checkoutHeaderDTO, "checkoutqueue");


                return Ok(checkoutHeaderDTO);
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(412);
            }
        }

        private async Task VerifyCoupon(CheckoutHeaderDTO checkoutHeader)
        {
            if (!string.IsNullOrEmpty(checkoutHeader.CouponCode))
            {
                string token = Request.Headers.Authorization;

                CouponDTO coupon = await _couponRepository.GetCouponByCouponCode(checkoutHeader.CouponCode, token);

                if (checkoutHeader.DiscountAmount != coupon.DiscountAmount)
                    throw new InvalidOperationException();
            }
        }
    }
}
