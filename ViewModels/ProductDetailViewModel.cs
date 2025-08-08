using ASM1.Models;

namespace FastFoodApp.ViewModels
{
    // Dùng để nhận dữ liệu từ form thêm vào giỏ hàng
    public class AddToCartViewModel
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; } = 1;

        // Các tùy chọn
        public bool IsSpicy { get; set; }
        public bool NoVegetables { get; set; }
        public bool ExtraSauce { get; set; }
    }

    // ViewModel chính cho trang chi tiết
    public class ProductDetailViewModel
    {
        public MonAn Product { get; set; } = null!;
    }
}
