using GeekShopping.ProductAPI.DTO;

namespace GeekShopping.ProductAPI.Repository.Interfaces
{
    public interface IProductRepository
    {
        public Task<ProductDTO> Create(ProductDTO productDTO);
        public Task<ProductDTO> Update(ProductDTO productDTO);
        public Task<bool> Remove(long id);
        public Task<ProductDTO> Get(long id);
        public Task<IEnumerable<ProductDTO>> GetAll();
    }
}