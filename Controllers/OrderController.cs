using FastFoodApp.Data;
using FastFoodApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace FastFoodApp.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string statusFilter = "all")
        {
            var userIdString = User.FindFirstValue("UserId");
            if (string.IsNullOrEmpty(userIdString))
            {
                return Unauthorized();
            }

            var userId = int.Parse(userIdString);

            var ordersQuery = _context.HoaDons
                .Include(h => h.NguoiDung)
                .Where(h => h.MaNguoiDung == userId);

            if (statusFilter != "all" && !string.IsNullOrEmpty(statusFilter))
            {
                ordersQuery = ordersQuery.Where(h => h.TrangThai == statusFilter);
            }

            var orders = await ordersQuery
                .Include(h => h.ChiTietHoaDons).ThenInclude(ct => ct.MonAn)
                .Include(h => h.ChiTietHoaDons).ThenInclude(ct => ct.Combo)
                .OrderByDescending(h => h.NgayDat)
                .Select(h => new OrderHistoryViewModel
                {
                    MaHoaDon = h.MaHoaDon,
                    NgayDat = h.NgayDat,
                    TongTien = h.TongTien,
                    TrangThai = h.TrangThai,
                    DiaChiGiaoHang = h.DiaChiGiaoHang,
                    SoDienThoai = h.NguoiDung.SoDienThoai,
                    Items = h.ChiTietHoaDons.Select(ct => new CartItemViewModel
                    {
                        ItemId = ct.MonAn != null ? ct.MonAn.MaMonAn : (ct.Combo != null ? ct.Combo.MaCombo : 0),
                        TenSanPham = ct.MonAn != null ? ct.MonAn.TenMonAn : (ct.Combo != null ? ct.Combo.TenCombo : "Sản phẩm không xác định"),
                        SoLuong = ct.SoLuong,
                        DonGia = ct.DonGia,
                        HinhAnh = Convert.ToBase64String(ct.MonAn != null ? (ct.MonAn.HinhAnh ?? new byte[0]) : (ct.Combo != null ? (ct.Combo.HinhAnh ?? new byte[0]) : new byte[0])),
                        IsCombo = ct.MaCombo.HasValue
                    }).ToList()
                })
                .ToListAsync();

            ViewData["CurrentFilter"] = statusFilter;
            return View(orders);
        }


        // GET: /Order/Details/5 (Trang theo dõi đơn hàng)
        // POST: /Order/Cancel/5 (Hủy đơn hàng)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id)
        {
            // Kiểm tra xem đơn hàng có tồn tại không và thuộc về người dùng hiện tại
            var userIdString = User.FindFirstValue("UserId");
            if (string.IsNullOrEmpty(userIdString))
            {
                return Unauthorized();
            }

            var userId = int.Parse(userIdString);
            var hoaDon = await _context.HoaDons
                .FirstOrDefaultAsync(h => h.MaHoaDon == id && h.MaNguoiDung == userId);

            if (hoaDon == null)
            {
                return NotFound();
            }

            // Chỉ cho phép hủy đơn hàng nếu đang ở trạng thái "Chờ xử lý" hoặc "Đang chuẩn bị"
            if (hoaDon.TrangThai == "Chờ xử lý" || hoaDon.TrangThai == "Đang chuẩn bị")
            {
                hoaDon.TrangThai = "Đã hủy";
                _context.Update(hoaDon);
                await _context.SaveChangesAsync();
                
                TempData["ToastSuccess"] = $"Đơn hàng #{id} đã được hủy thành công!";
                return RedirectToAction(nameof(Index));
            }
            
            TempData["ToastError"] = "Không thể hủy đơn hàng ở trạng thái hiện tại.";
            return RedirectToAction(nameof(Details), new { id });
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userIdString = User.FindFirstValue("UserId");
            if (string.IsNullOrEmpty(userIdString))
            {
                return Unauthorized();
            }
            
            var userId = int.Parse(userIdString);

            var hoaDon = await _context.HoaDons
                .Include(h => h.NguoiDung)
                .Include(h => h.ChiTietHoaDons).ThenInclude(ct => ct.MonAn)
                .Include(h => h.ChiTietHoaDons).ThenInclude(ct => ct.Combo)
                .FirstOrDefaultAsync(h => h.MaHoaDon == id && h.MaNguoiDung == userId);

            if (hoaDon == null)
            {
                return NotFound();
            }

            // Tạo các bước theo dõi
            var allSteps = new List<Tuple<string, string, string>>
            {
                Tuple.Create("Chờ xử lý", "Đơn hàng đã được tiếp nhận", "Hệ thống đã ghi nhận đơn hàng của bạn."),
                Tuple.Create("Đang chuẩn bị", "Đang chuẩn bị món ăn", "Nhà bếp đang chuẩn bị những món ăn nóng hổi."),
                Tuple.Create("Sẵn sàng", "Món ăn đã sẵn sàng", "Các món ăn đã được chuẩn bị xong và chờ giao đi."),
                Tuple.Create("Đang giao", "Đang giao hàng", "Tài xế đang trên đường giao hàng đến bạn."),
                Tuple.Create("Đã giao", "Đã giao hàng thành công", "Cảm ơn bạn đã tin tưởng SnapBite!")
            };

            var currentStepIndex = allSteps.FindIndex(s => s.Item1.Equals(hoaDon.TrangThai, StringComparison.OrdinalIgnoreCase));

            var trackingSteps = new List<TrackingStepViewModel>();
            for (int i = 0; i < allSteps.Count; i++)
            {
                trackingSteps.Add(new TrackingStepViewModel
                {
                    Status = allSteps[i].Item1,
                    Title = allSteps[i].Item2,
                    Description = allSteps[i].Item3,
                    Timestamp = i <= currentStepIndex ? hoaDon.NgayDat.AddMinutes(i * 5) : (DateTime?)null, // Giả lập thời gian
                    IsCompleted = i < currentStepIndex,
                    IsCurrent = i == currentStepIndex
                });
            }

            var model = new OrderTrackingViewModel
            {
                MaHoaDon = hoaDon.MaHoaDon,
                NgayDat = hoaDon.NgayDat,
                ThoiGianGiaoHangDuKien = hoaDon.NgayDat.AddMinutes(30),
                HoTenKhachHang = hoaDon.NguoiDung.HoTen,
                SoDienThoai = hoaDon.NguoiDung.SoDienThoai,
                DiaChiGiaoHang = hoaDon.DiaChiGiaoHang,
                TongTien = hoaDon.TongTien,
                PhuongThucThanhToan = "Tiền mặt khi nhận hàng", // Cần thêm trường này vào DB nếu muốn đa dạng
                Items = hoaDon.ChiTietHoaDons.Select(ct => new CartItemViewModel
                {
                    TenSanPham = ct.MonAn?.TenMonAn ?? ct.Combo?.TenCombo ?? "Sản phẩm không xác định",
                    SoLuong = ct.SoLuong,
                    DonGia = ct.DonGia,
                    HinhAnh = Convert.ToBase64String(ct.MonAn?.HinhAnh ?? ct.Combo?.HinhAnh ?? new byte[0]),
                }).ToList(),
                TrackingSteps = trackingSteps
            };

            return View(model);
        }
    }
}
