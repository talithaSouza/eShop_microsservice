using System.Net.Http.Headers;
using GeekShopping_Web.Models;
using GeekShopping_Web.Services.IServices;

namespace GeekShopping_Web.Services
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _client;
        public const string BasePath = "api/v1/products";

        public ProductService(HttpClient client)
        {
            _client = client;
        }

        private void SetToken(string token)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
        public async Task<IEnumerable<ProductViewModel>> GetAllProducts(string token)
        {
            
            var response = await _client.GetAsync(BasePath);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<IEnumerable<ProductViewModel>>();
        }

        public async Task<ProductViewModel> GetProduct(long id, string token)
        {
            SetToken(token);

            var response = await _client.GetAsync($"{BasePath}/{id}");

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<ProductViewModel>();
        }

        public async Task<ProductViewModel> CreateProduct(ProductViewModel productModel, string token)
        {
            SetToken(token);

            var jsonContent = JsonContent.Create(productModel);

            var response = await _client.PostAsync(BasePath, jsonContent);

            if (!response.IsSuccessStatusCode)
                throw new Exception("Falha ao desserializar a resposta no Post.");

            return await response.Content.ReadFromJsonAsync<ProductViewModel>();
        }

        public async Task<ProductViewModel> UpdateProduct(ProductViewModel productModel, string token)
        {
            SetToken(token);

            var jsonContent = JsonContent.Create(productModel);

            var response = await _client.PutAsync(BasePath, jsonContent);

            if (!response.IsSuccessStatusCode)
                throw new Exception("Falha ao desserializar a resposta no Post.");

            return await response.Content.ReadFromJsonAsync<ProductViewModel>();
        }

        public async Task<bool> RemoveProductById(long id, string token)
        {
            SetToken(token);

            var response = await _client.DeleteAsync($"{BasePath}/{id}");

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<bool>();
        }
    }
}