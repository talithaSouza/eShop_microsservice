using GeekShopping_Web.Models;

namespace GeekShopping_Web.Services.IServices
{
    public interface IProductService
    {
        public Task<ProductModel> GetProduct(long id, string token);
        public Task<IEnumerable<ProductModel>> GetAllProducts(string token);
        public Task<ProductModel> CreateProduct(ProductModel productModel, string token);
        public Task<ProductModel> UpdateProduct(ProductModel productModel, string token);
        public Task<bool> RemoveProductById(long id, string token);
    }
}