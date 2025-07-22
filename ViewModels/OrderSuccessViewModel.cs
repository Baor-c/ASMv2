using System;
using System.Collections.Generic;

namespace FastFoodApp.ViewModels
{
    public class OrderSuccessViewModel
    {
        public int MaHoaDon { get; set; }
        public DateTime NgayDat { get; set; }
        public DateTime ThoiGianGiaoHangDuKien { get; set; }
        public string DiaChiGiaoHang { get; set; }
        public string SoDienThoai { get; set; }
        public string PhuongThucThanhToan { get; set; }
        public decimal TongTienThanhToan { get; set; }
        public List<CartItemViewModel> Items { get; set; }

        public OrderSuccessViewModel()
        {
            Items = new List<CartItemViewModel>();
        }
    }
}
