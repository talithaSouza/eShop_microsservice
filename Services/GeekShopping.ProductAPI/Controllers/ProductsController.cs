using GeekShopping.ProductAPI.DTO;
using GeekShopping.ProductAPI.Model;
using GeekShopping.ProductAPI.Repository.Interfaces;
using GeekShopping.ProductAPI.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeekShopping.ProductAPI.Controllers
{

    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _repository;

        public ProductsController(IProductRepository repository)
        {
            _repository = repository;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _repository.GetAll());
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            ProductDTO product = await _repository.Get(id);

            if (product == null)
                return NotFound();


            return Ok(product);
        }

        [Authorize]
        [HttpPost()]
        public async Task<IActionResult> Post([FromBody] ProductDTO productDTO)
        {
            if (productDTO == null)
                return BadRequest();

            return Ok(await _repository.Create(productDTO));
        }

        [Authorize]
        [HttpPut()]
        public async Task<IActionResult> Put([FromBody] ProductDTO productDTO)
        {
            if (productDTO == null)
                return BadRequest();

            return Ok(await _repository.Update(productDTO));
        }

        [Authorize(Roles = Role.Admin)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            bool deleted = await _repository.Remove(id);

            if (deleted)
                return Ok(deleted);


            return NotFound();
        }
    }
}
