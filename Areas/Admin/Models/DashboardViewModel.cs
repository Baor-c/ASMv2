using System;
using System.Collections.Generic;

namespace FastFoodApp.Areas.Admin.Models
{
    public class DashboardViewModel
    {
        public decimal TotalRevenue { get; set; }
        public int TotalOrders { get; set; }
        public int NewOrdersToday { get; set; }
        public int TotalUsers { get; set; }
        public List<BestSellerViewModel> BestSellingItems { get; set; }
        public List<RevenueByDateViewModel> DailyRevenue { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class BestSellerViewModel
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string ItemType { get; set; }
        public int TotalQuantitySold { get; set; }
    }

    // Đổi tên để phản ánh đúng dữ liệu theo ngày
    public class RevenueByDateViewModel
    {
        public string Date { get; set; } // "2025-07-25"
        public decimal Revenue { get; set; }
    }
}
