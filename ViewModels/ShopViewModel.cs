using ASM1.Models;
using System.Collections.Generic;

namespace FastFoodApp.ViewModels
{
    public class ShopViewModel
    {
        public List<MonAn> Products { get; set; } = new List<MonAn>();
        public List<LoaiMonAn> Categories { get; set; } = new List<LoaiMonAn>();
        public List<Combo> Combos { get; set; } = new List<Combo>();

        // Tìm kiếm và lọc cơ bản
        public string? SearchTerm { get; set; }
        public List<int> CategoryIds { get; set; } = new List<int>(); // Đổi từ CategoryId thành CategoryIds để hỗ trợ multiple selection
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }

        // Lọc theo đặc tính sản phẩm
        public bool? IsPopular { get; set; }
        public bool? IsNew { get; set; }
        
        // Sắp xếp
        public string SortBy { get; set; } = "newest"; // newest, price_asc, price_desc, popular, rating
        
        // Phân trang
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; } = 12; // Tăng từ 8 lên 12 để hiển thị nhiều sản phẩm hơn
        public int TotalItems { get; set; }
        
        // Thông tin phụ trợ
        public decimal MinPriceAvailable { get; set; }
        public decimal MaxPriceAvailable { get; set; }
        
        // Các dropdown/checkbox values
        public List<string> AvailableBrands { get; set; } = new List<string>(); // Tạm thời để trống, có thể mở rộng sau
    }
}
