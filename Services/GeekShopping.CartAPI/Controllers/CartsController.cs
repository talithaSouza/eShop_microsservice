
using GeekShopping.CartAPI.DTO;
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
    }
}
