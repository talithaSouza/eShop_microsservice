using System.ComponentModel.DataAnnotations;

namespace GeekShopping_Web.Models
{
    public class CartHeaderViewModel
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public int Id { get; set; }

        [Required]
        public string CouponCode { get; set; }

        [Required]
        public decimal DiscountAmount { get; set; }

        [Required]
        public decimal PurchaseAmount { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Card number is required.")]
        public string CardNumber { get; set; }

        [Required(ErrorMessage = "CVV is required.")]
        [StringLength(4, MinimumLength = 3, ErrorMessage = "CVV must be 3 or 4 digits.")]
        public string CVV { get; set; }

        [Required(ErrorMessage = "Card expiry is required.")]
        [RegularExpression(@"^(0[1-9]|1[0-2])([0-9]{2})$", ErrorMessage = "Expiry must be in MMYY format.")]
        public string ExpiryMothYear { get; set; }
    }
}
