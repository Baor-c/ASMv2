using System;
using System.Collections.Generic;

namespace FastFoodApp.ViewModels
{
    // Đại diện cho một bước trong quy trình theo dõi
    public class TrackingStepViewModel
    {
        public string Status { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? Timestamp { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsCurrent { get; set; }
    }

    public class OrderTrackingViewModel
    {
        public int MaHoaDon { get; set; }
        public DateTime NgayDat { get; set; }
        public DateTime ThoiGianGiaoHangDuKien { get; set; }
        public string HoTenKhachHang { get; set; }
        public string SoDienThoai { get; set; }
        public string DiaChiGiaoHang { get; set; }
        public string PhuongThucThanhToan { get; set; }
        public decimal TongTien { get; set; }
        public List<CartItemViewModel> Items { get; set; }
        public List<TrackingStepViewModel> TrackingSteps { get; set; }

        public OrderTrackingViewModel()
        {
            Items = new List<CartItemViewModel>();
            TrackingSteps = new List<TrackingStepViewModel>();
        }
    }
}
