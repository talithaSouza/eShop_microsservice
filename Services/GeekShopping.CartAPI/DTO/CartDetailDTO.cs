namespace GeekShopping.CartAPI.DTO
{
    public class CartDetailDTO
    {
        public long Id { get; set; }
        public long CartHeaderId { get; set; }

        public long CartId { get; set; }
        public long ProductId { get; set; }
        public int Count { get; set; }
        public CartHeaderDTO CartHeader { get; set; }
        public ProductDTO Product { get; set; }
    }
}