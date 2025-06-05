using AutoMapper;
using GeekShopping.ProductAPI.DTO;
using GeekShopping.ProductAPI.Model;
using GeekShopping.ProductAPI.Model.Context;
using GeekShopping.ProductAPI.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GeekShopping.ProductAPI.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly MySqlContext _context;
        private readonly IMapper _mapper;

        public ProductRepository(MySqlContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<IEnumerable<ProductDTO>> GetAll()
        {
            var products = await _context.Products.ToListAsync();

            return _mapper.Map<IEnumerable<ProductDTO>>(products);
        }

        public async Task<ProductDTO> Get(long id)
        {
            return _mapper.Map<ProductDTO>(await _context.Products.FindAsync(id));
        }

        public async Task<ProductDTO> Create(ProductDTO productDTO)
        {
            var productEntity = _mapper.Map<Product>(productDTO);

            _context.Products.Add(productEntity);

            await _context.SaveChangesAsync();

            return _mapper.Map<ProductDTO>(productEntity);
        }
        public async Task<ProductDTO> Update(ProductDTO productDTO)
        {
            var productEntity = _mapper.Map<Product>(productDTO);

            _context.Products.Update(productEntity);

            await _context.SaveChangesAsync();

            return productDTO;
        }

        public async Task<bool> Remove(long id)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);

                if (product == null)
                    return false;

                _context.Products.Remove(product);

                await _context.SaveChangesAsync();

                return true;

            }
            catch (Exception)
            {
                return false;
            }
        }

    }

}

