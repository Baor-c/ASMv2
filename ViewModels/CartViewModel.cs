using ASM1.Models;
using System.Collections.Generic;

namespace FastFoodApp.ViewModels
{
    public class CartViewModel
    {
        public List<CartItemViewModel> CartItems { get; set; }
        public List<MonAn> RecommendedProducts { get; set; } 

        public decimal Subtotal => CartItems.Sum(item => item.ThanhTien);
        public decimal Total => Subtotal; 

        public CartViewModel()
        {
            CartItems = new List<CartItemViewModel>();
            RecommendedProducts = new List<MonAn>();
        }
    }
}