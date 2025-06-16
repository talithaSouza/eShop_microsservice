using GeekShopping_Web.Models;

namespace GeekShopping_Web.Services.IServices
{
    public interface IProductService
    {
        public Task<ProductViewModel> GetProduct(long id, string token);
        public Task<IEnumerable<ProductViewModel>> GetAllProducts(string token);
        public Task<ProductViewModel> CreateProduct(ProductViewModel productModel, string token);
        public Task<ProductViewModel> UpdateProduct(ProductViewModel productModel, string token);
        public Task<bool> RemoveProductById(long id, string token);
    }
}