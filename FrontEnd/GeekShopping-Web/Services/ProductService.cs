using GeekShopping_Web.Models;
using GeekShopping_Web.Services.IServices;

namespace GeekShopping_Web.Services
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _client;
        public const string BasePath = "api/v1/product";

        public ProductService(HttpClient client)
        {
            _client = client;
        }


        public async Task<IEnumerable<ProductModel>> GetAllProducts()
        {
            var response = await _client.GetAsync(BasePath);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<IEnumerable<ProductModel>>();
        }

        public async Task<ProductModel> GetProduct(long id)
        {
            var response = await _client.GetAsync($"{BasePath}/{id}");

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<ProductModel>();
        }

        public async Task<ProductModel> CreateProduct(ProductModel productModel)
        {
            var jsonContent = JsonContent.Create(productModel);

            var response = await _client.PostAsync(BasePath, jsonContent);

            if (!response.IsSuccessStatusCode)
                throw new Exception("Falha ao desserializar a resposta no Post.");

            return await response.Content.ReadFromJsonAsync<ProductModel>();
        }

        public async Task<ProductModel> UpdateProduct(ProductModel productModel)
        {
             var jsonContent = JsonContent.Create(productModel);

            var response = await _client.PutAsync(BasePath, jsonContent);

            if (!response.IsSuccessStatusCode)
                throw new Exception("Falha ao desserializar a resposta no Post.");

            return await response.Content.ReadFromJsonAsync<ProductModel>();
        }

        public async Task<bool> RemoveProductById(long id)
        {
             var response = await _client.DeleteAsync($"{BasePath}/{id}");

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<bool>();
        }
    }
}