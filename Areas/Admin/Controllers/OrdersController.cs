// File: Areas/Admin/Controllers/OrdersController.cs
using FastFoodApp.Data;
using ASM1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using FastFoodApp.Areas.Admin.Models;

namespace FastFoodApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(
            string sortOrder,
            string currentSearchTerm,
            string searchTerm,
            string currentStatusFilter,
            string statusFilter,
            DateTime? currentStartDate,
            DateTime? startDate,
            DateTime? currentEndDate,
            DateTime? endDate,
            int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["DateSortParm"] = String.IsNullOrEmpty(sortOrder) ? "date_desc" : "";
            ViewData["TotalSortParm"] = sortOrder == "Total" ? "total_desc" : "Total";

            // Nếu có một bộ lọc mới được áp dụng, reset về trang 1
            if (searchTerm != null || statusFilter != null || startDate != null || endDate != null)
            {
                pageNumber = 1;
            }
            else // Ngược lại, giữ lại các bộ lọc từ trang trước
            {
                searchTerm = currentSearchTerm;
                statusFilter = currentStatusFilter;
                startDate = currentStartDate;
                endDate = currentEndDate;
            }

            // Lưu lại các giá trị lọc hiện tại để truyền cho các liên kết phân trang/sắp xếp
            ViewData["CurrentSearchTerm"] = searchTerm;
            ViewData["CurrentStatusFilter"] = statusFilter;
            ViewData["CurrentStartDate"] = startDate?.ToString("yyyy-MM-dd");
            ViewData["CurrentEndDate"] = endDate?.ToString("yyyy-MM-dd");

            var ordersQuery = _context.HoaDons.Include(h => h.NguoiDung).AsQueryable();

            // Áp dụng các bộ lọc
            if (!String.IsNullOrEmpty(searchTerm))
            {
                ordersQuery = ordersQuery.Where(o => o.NguoiDung.HoTen.Contains(searchTerm) || o.MaHoaDon.ToString().Contains(searchTerm));
            }

            if (!String.IsNullOrEmpty(statusFilter))
            {
                ordersQuery = ordersQuery.Where(o => o.TrangThai == statusFilter);
            }

            if (startDate.HasValue)
            {
                ordersQuery = ordersQuery.Where(o => o.NgayDat.Date >= startDate.Value.Date);
            }

            if (endDate.HasValue)
            {
                ordersQuery = ordersQuery.Where(o => o.NgayDat.Date <= endDate.Value.Date);
            }

            // Áp dụng sắp xếp
            switch (sortOrder)
            {
                case "date_desc":
                    ordersQuery = ordersQuery.OrderByDescending(o => o.NgayDat);
                    break;
                case "Total":
                    ordersQuery = ordersQuery.OrderBy(o => o.TongTien);
                    break;
                case "total_desc":
                    ordersQuery = ordersQuery.OrderByDescending(o => o.TongTien);
                    break;
                default: // Mặc định sắp xếp theo ngày mới nhất
                    ordersQuery = ordersQuery.OrderByDescending(o => o.NgayDat);
                    break;
            }

            int pageSize = 10;
            var paginatedList = await PaginatedList<HoaDon>.CreateAsync(ordersQuery.AsNoTracking(), pageNumber ?? 1, pageSize);

            return View(paginatedList);
        }

        // Action Details (không thay đổi)
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var hoaDon = await _context.HoaDons
                .Include(h => h.NguoiDung)
                .Include(h => h.ChiTietHoaDons).ThenInclude(ct => ct.MonAn)
                .Include(h => h.ChiTietHoaDons).ThenInclude(ct => ct.Combo)
                .FirstOrDefaultAsync(m => m.MaHoaDon == id);
            if (hoaDon == null) return NotFound();
            return View(hoaDon);
        }

        // Action UpdateStatus (cập nhật để giữ lại tất cả bộ lọc)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int orderId, string newStatus, int? pageNumber, string sortOrder, string currentSearchTerm, string currentStatusFilter, DateTime? currentStartDate, DateTime? currentEndDate)
        {
            var order = await _context.HoaDons.FindAsync(orderId);
            if (order != null)
            {
                order.TrangThai = newStatus;
                _context.Update(order);
                await _context.SaveChangesAsync();
                TempData["ToastSuccess"] = $"Đã cập nhật trạng thái đơn hàng #{orderId}.";
            }

            return RedirectToAction(nameof(Index), new
            {
                pageNumber = pageNumber,
                sortOrder = sortOrder,
                searchTerm = currentSearchTerm,
                statusFilter = currentStatusFilter,
                startDate = currentStartDate,
                endDate = currentEndDate
            });
        }
    }
}
