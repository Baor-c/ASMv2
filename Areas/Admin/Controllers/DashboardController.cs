using FastFoodApp.Data;
using FastFoodApp.Areas.Admin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Globalization;
using System.Collections.Generic;

namespace FastFoodApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(DateTime? startDate, DateTime? endDate)
        {
            // Thiết lập khoảng thời gian mặc định là 30 ngày gần nhất
            var finalEndDate = endDate ?? DateTime.Today;
            var finalStartDate = startDate ?? finalEndDate.AddDays(-29);

            // Đảm bảo ngày kết thúc không trước ngày bắt đầu
            if (finalEndDate < finalStartDate)
            {
                finalEndDate = finalStartDate;
            }

            // Tạo một truy vấn cơ sở cho các đơn hàng trong khoảng thời gian đã chọn
            var ordersInRange = _context.HoaDons
                .Where(h => h.NgayDat.Date >= finalStartDate.Date && h.NgayDat.Date <= finalEndDate.Date);

            // Tính toán các chỉ số dựa trên dữ liệu đã lọc
            var totalRevenue = await ordersInRange
                .Where(h => h.TrangThai == "Đã giao")
                .SumAsync(h => (decimal?)h.TongTien) ?? 0;

            var totalOrders = await ordersInRange.CountAsync();

            // Các chỉ số này không bị ảnh hưởng bởi bộ lọc ngày
            var newOrdersToday = await _context.HoaDons.CountAsync(h => h.NgayDat.Date == DateTime.Today);
            var totalUsers = await _context.NguoiDungs.CountAsync(u => u.VaiTro.TenVaiTro == "Customer");

            // Lấy dữ liệu cho biểu đồ doanh thu theo ngày
            var dailyRevenueData = await _context.HoaDons
                .Where(h => h.TrangThai == "Đã giao" && h.NgayDat.Date >= finalStartDate.Date && h.NgayDat.Date <= finalEndDate.Date)
                .GroupBy(h => h.NgayDat.Date)
                .Select(g => new
                {
                    Date = g.Key,
                    Revenue = g.Sum(h => h.TongTien)
                })
                .OrderBy(r => r.Date)
                .ToListAsync();

            // Sản phẩm bán chạy (có thể tính trong khoảng thời gian hoặc toàn thời gian)
            var bestSellingItems = await GetBestSellingItems(); 

            var viewModel = new DashboardViewModel
            {
                TotalRevenue = totalRevenue,
                TotalOrders = totalOrders,
                NewOrdersToday = newOrdersToday,
                TotalUsers = totalUsers,
                BestSellingItems = bestSellingItems,
                DailyRevenue = dailyRevenueData.Select(r => new RevenueByDateViewModel
                {
                    Date = r.Date.ToString("yyyy-MM-dd"),
                    Revenue = r.Revenue
                }).ToList(),
                StartDate = finalStartDate,
                EndDate = finalEndDate
            };

            return View(viewModel);
        }

        private async Task<List<BestSellerViewModel>> GetBestSellingItems()
        {
            var bestSellingMonAn = await _context.ChiTietHoaDons
                .Where(ct => ct.MaMonAn != null)
                .GroupBy(ct => ct.MonAn.TenMonAn)
                .Select(g => new BestSellerViewModel
                {
                    ItemName = g.Key,
                    ItemType = "Món ăn",
                    TotalQuantitySold = g.Sum(ct => ct.SoLuong)
                })
                .ToListAsync();

            var bestSellingCombos = await _context.ChiTietHoaDons
                .Where(ct => ct.MaCombo != null)
                .GroupBy(ct => ct.Combo.TenCombo)
                .Select(g => new BestSellerViewModel
                {
                    ItemName = g.Key,
                    ItemType = "Combo",
                    TotalQuantitySold = g.Sum(ct => ct.SoLuong)
                })
                .ToListAsync();

            return bestSellingMonAn.Concat(bestSellingCombos)
                .OrderByDescending(i => i.TotalQuantitySold)
                .Take(5) // Lấy top 5
                .ToList();
        }
    }
}
