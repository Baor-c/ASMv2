using System;
using System.Collections.Generic;

namespace FastFoodApp.ViewModels
{
    public class OrderDetailViewModel
    {
        public int MaHoaDon { get; set; }
        public DateTime NgayDat { get; set; }
        public string TrangThai { get; set; }
        public string DiaChiGiaoHang { get; set; }
        public decimal TongTien { get; set; }
        public List<CartItemViewModel> ChiTietDonHang { get; set; }

        public OrderDetailViewModel()
        {
            ChiTietDonHang = new List<CartItemViewModel>();
        }
    }
}
