using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASM1.Models
{
    public class CartItem
    {
        [Key]
        public int CartItemID { get; set; }

        public string CartSessionID { get; set; } = null!; // Or UserID if users are logged in

        public int ProductID { get; set; }
        [ForeignKey("ProductID")]
        public virtual MonAn Product { get; set; } = null!;

        public int Quantity { get; set; }

        public decimal FinalPrice { get; set; } // Price per unit
    }
}
