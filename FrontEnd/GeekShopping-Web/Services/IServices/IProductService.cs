using GeekShopping_Web.Models;

namespace GeekShopping_Web.Services.IServices
{
    public interface IProductService
    {
        public Task<ProductModel> GetProduct(long id);
        public Task<IEnumerable<ProductModel>> GetAllProducts();
        public Task<ProductModel> CreateProduct(ProductModel productModel);
        public Task<ProductModel> UpdateProduct(ProductModel productModel);
        public Task<bool> RemoveProductById(long id);
    }
}