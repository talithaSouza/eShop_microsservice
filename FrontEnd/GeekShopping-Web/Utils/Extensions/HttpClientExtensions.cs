using System.Net.Http.Headers;
using System.Text.Json;

namespace GeekShopping_Web.Utils.Extensions
{
    public static class HttpClientExtensions
    {
        private static MediaTypeHeaderValue contentType = new MediaTypeHeaderValue("application/json");
        public static async Task<T> GetAsync<T>(this HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
                throw new ApplicationException($"Aconteceu algum erro durante a chamada da API: {response.ReasonPhrase}");

            var dataAsString = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);

            return JsonSerializer.Deserialize<T>(dataAsString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public static Task<HttpResponseMessage> Post<T>(this HttpClient client, string url, T data)
        {
            var content = new StringContent(JsonSerializer.Serialize(data));

            content.Headers.ContentType = contentType;

            return client.PostAsync(url, content);
        }

        public static Task<HttpResponseMessage> Put<T>(this HttpClient client, string url, T data)
        {
            var dataAsString = JsonSerializer.Serialize(data);
            var content = new StringContent(dataAsString, contentType);

            return client.PutAsync(url, content);
        }

        public static async Task<T> ReadContentAs<T>(this HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
                throw new ApplicationException($"Something went wrong calling the API: {response.ReasonPhrase}");


            var dataAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            return JsonSerializer.Deserialize<T>(dataAsString,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
        }
    }
}