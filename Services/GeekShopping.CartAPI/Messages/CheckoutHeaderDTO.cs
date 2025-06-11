using GeekShopping.CartAPI.DTO;

namespace GeekShopping.CartAPI.Messages
{
    public class CheckoutHeader
    {
        public long Id { get; set; }
        public string UserId { get; set; }
        public string CouponCode { get; set; }
        public decimal PurchaseAmount { get; set; }

        public decimal DescountAmount { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateTime { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string CVV { get; set; }
        public string ExpiryMothYear { get; set; }

        public int CartTotal { get; set; }
        public IEnumerable<CartDetailDTO> CartDetails { get; set; }
    }
}