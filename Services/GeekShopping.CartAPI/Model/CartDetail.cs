using System.ComponentModel.DataAnnotations.Schema;
using GeekShopping.CartAPI.Model.Base;

namespace GeekShopping.CartAPI.Model
{
    [Table("cart_detail")]
    public class CartDetail : BaseEntity
    {
        [Column("cart_header_id")]
        public long CartHeaderId { get; set; }

        [Column("product_id")]
        public long ProductId { get; set; }

        [Column("count")]    
        public int Count { get; set; }


        //FKs
        [ForeignKey(nameof(CartHeaderId))]
        public virtual CartHeader CartHeader { get; set; }

        [ForeignKey(nameof(ProductId))]
        public virtual Product Product { get; set; }
    }
}