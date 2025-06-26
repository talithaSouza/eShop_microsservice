using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using AutoMapper;
using GeekShopping.CartAPI.DTO;
using GeekShopping.CartAPI.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GeekShopping.CartAPI.Repository
{
    public class CouponRepository : ICouponRepository
    {
        private readonly HttpClient _client;
        public const string BasePath = "api/v1/coupon";

        public CouponRepository(HttpClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }
        public async Task<CouponDTO> GetCouponByCouponCode(string couponCode, string token)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.GetAsync($"{BasePath}/{couponCode}");

            if (response.StatusCode != HttpStatusCode.OK)
                new CouponDTO();

            return JsonSerializer.Deserialize<CouponDTO>(await response.Content.ReadAsStringAsync());
        }
    }
}
