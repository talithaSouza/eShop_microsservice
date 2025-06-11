using System.ComponentModel.DataAnnotations.Schema;
using GeekShopping.CartAPI.Model.Base;

namespace GeekShopping.CartAPI.Model
{
    [Table("cart_detail")]
    public class CartDetail : BaseEntity
    {
        [Column("user_id")]
        public long CartHeaderId { get; set; }

        public long CartId { get; set; }
        public long ProductId { get; set; }

        [Column("count")]    
        public int Count { get; set; }


        //FKs
        [ForeignKey(nameof(CartHeaderId))]
        public CartHeader CartHeader { get; set; }

        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; }
    }
}