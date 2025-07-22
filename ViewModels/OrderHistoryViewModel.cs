using System;
using System.Collections.Generic;

namespace FastFoodApp.ViewModels
{
    public class OrderHistoryViewModel
    {
        public int MaHoaDon { get; set; }
        public DateTime NgayDat { get; set; }
        public decimal TongTien { get; set; }
        public string TrangThai { get; set; }
        public List<CartItemViewModel> Items { get; set; }
        public string DiaChiGiaoHang { get; set; }
        public string SoDienThoai { get; set; } // Thêm SĐT
    }
}
