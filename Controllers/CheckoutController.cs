using ASM1.Models;
using FastFoodApp.Data;
using FastFoodApp.Helpers;
using FastFoodApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FastFoodApp.Controllers
{
    [Authorize]
    public class CheckoutController : Controller
    {
        private readonly ApplicationDbContext _context;
        public const string CARTKEY = "cart";

        public CheckoutController(ApplicationDbContext context)
        {
            _context = context;
        }

        List<CartItemViewModel> Carts => HttpContext.Session.Get<List<CartItemViewModel>>(CARTKEY) ?? new List<CartItemViewModel>();

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var gioHang = Carts;
            if (gioHang.Count == 0)
            {
                return RedirectToAction("Index", "Cart");
            }

            var userId = int.Parse(User.FindFirstValue("UserId"));
            var userAddresses = await _context.DiaChiNguoiDungs.Where(a => a.MaNguoiDung == userId).AsNoTracking().ToListAsync();

            if (!userAddresses.Any())
            {
                TempData["ErrorMessage"] = "Bạn cần thêm địa chỉ giao hàng trong trang cá nhân trước khi đặt hàng.";
                return RedirectToAction("Profile", "Account");
            }

            var user = await _context.NguoiDungs.Include(u => u.CapThanhVien).AsNoTracking().FirstOrDefaultAsync(u => u.MaNguoiDung == userId);
            if (user == null) return Challenge();

            var tongTienGoc = gioHang.Sum(p => p.ThanhTien);
            var phanTramGiamGia = user.CapThanhVien?.PhanTramGiamGia ?? 0;
            var soTienGiam = (decimal)(phanTramGiamGia / 100) * tongTienGoc;
            var tongTienThanhToan = tongTienGoc - soTienGiam;

            var model = new CheckoutViewModel
            {
                Addresses = userAddresses,
                SelectedAddressId = userAddresses.FirstOrDefault(a => a.IsDefault)?.MaDiaChi ?? userAddresses.First().MaDiaChi,
                CartItems = gioHang,
                TongTienGoc = tongTienGoc,
                PhanTramGiamGia = phanTramGiamGia,
                SoTienGiam = soTienGiam,
                TongTienThanhToan = tongTienThanhToan
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(CheckoutViewModel model)
        {
            var userId = int.Parse(User.FindFirstValue("UserId"));
            var gioHang = Carts;

            if (gioHang.Count == 0) return RedirectToAction("Index", "Cart");
            if (model.SelectedAddressId == 0) ModelState.AddModelError("SelectedAddressId", "Vui lòng chọn một địa chỉ giao hàng.");

            if (ModelState.IsValid)
            {
                var user = await _context.NguoiDungs.Include(u => u.CapThanhVien).FirstOrDefaultAsync(u => u.MaNguoiDung == userId);
                var selectedAddress = await _context.DiaChiNguoiDungs.FirstOrDefaultAsync(a => a.MaDiaChi == model.SelectedAddressId && a.MaNguoiDung == userId);
                if (user == null || selectedAddress == null) return NotFound();

                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        var tongTienGoc = gioHang.Sum(p => p.ThanhTien);
                        var phanTramGiamGia = user.CapThanhVien?.PhanTramGiamGia ?? 0;
                        var soTienGiam = (decimal)(phanTramGiamGia / 100) * tongTienGoc;
                        var tongTienThanhToan = tongTienGoc - soTienGiam;

                        var hoaDon = new HoaDon
                        {
                            MaNguoiDung = userId,
                            NgayDat = DateTime.Now,
                            DiaChiGiaoHang = selectedAddress.DiaChiCuThe, // Sử dụng địa chỉ đã chọn
                            TrangThai = "Chờ xử lý",
                            TongTien = tongTienThanhToan
                        };
                        _context.Add(hoaDon);
                        await _context.SaveChangesAsync();

                        foreach (var item in gioHang)
                        {
                            var chiTiet = new ChiTietHoaDon { MaHoaDon = hoaDon.MaHoaDon, SoLuong = item.SoLuong, DonGia = item.DonGia, MaCombo = item.IsCombo ? item.ItemId : (int?)null, MaMonAn = !item.IsCombo ? item.ItemId : (int?)null };
                            _context.Add(chiTiet);
                        }

                        int diemCongThem = (int)(tongTienThanhToan / 10000);
                        user.DiemTichLuy += diemCongThem;

                        var capThanhVienMoi = await _context.CapThanhViens.Where(c => c.NguongDiem <= user.DiemTichLuy).OrderByDescending(c => c.NguongDiem).FirstOrDefaultAsync();
                        if (capThanhVienMoi != null && capThanhVienMoi.MaCapThanhVien != user.MaCapThanhVien)
                        {
                            user.MaCapThanhVien = capThanhVienMoi.MaCapThanhVien;
                        }

                        _context.Update(user);
                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();
                        HttpContext.Session.Remove(CARTKEY);

                        return RedirectToAction("OrderSuccess", new { id = hoaDon.MaHoaDon });
                    }
                    catch (Exception)
                    {
                        await transaction.RollbackAsync();
                        ModelState.AddModelError("", "Đã có lỗi xảy ra trong quá trình đặt hàng. Vui lòng thử lại.");
                    }
                }
            }

            // Nếu model không hợp lệ, tạo lại ViewModel đầy đủ để trả về View
            var userForViewModel = await _context.NguoiDungs.Include(u => u.CapThanhVien).AsNoTracking().FirstOrDefaultAsync(u => u.MaNguoiDung == userId);
            var userAddresses = await _context.DiaChiNguoiDungs.Where(a => a.MaNguoiDung == userId).AsNoTracking().ToListAsync();

            var tongTienGocKhiLoi = gioHang.Sum(p => p.ThanhTien);
            var phanTramGiamGiaKhiLoi = userForViewModel.CapThanhVien?.PhanTramGiamGia ?? 0;
            var soTienGiamKhiLoi = (decimal)(phanTramGiamGiaKhiLoi / 100) * tongTienGocKhiLoi;

            model.Addresses = userAddresses;
            model.CartItems = gioHang;
            model.TongTienGoc = tongTienGocKhiLoi;
            model.PhanTramGiamGia = phanTramGiamGiaKhiLoi;
            model.SoTienGiam = soTienGiamKhiLoi;
            model.TongTienThanhToan = tongTienGocKhiLoi - soTienGiamKhiLoi;

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> OrderSuccess(int id)
        {
            var userId = int.Parse(User.FindFirstValue("UserId"));
            var order = await _context.HoaDons.Include(h => h.NguoiDung).Include(h => h.ChiTietHoaDons).ThenInclude(ct => ct.MonAn).Include(h => h.ChiTietHoaDons).ThenInclude(ct => ct.Combo).FirstOrDefaultAsync(h => h.MaHoaDon == id && h.MaNguoiDung == userId);
            if (order == null) return NotFound();

            var viewModel = new OrderSuccessViewModel
            {
                MaHoaDon = order.MaHoaDon,
                NgayDat = order.NgayDat,
                ThoiGianGiaoHangDuKien = order.NgayDat.AddMinutes(30),
                DiaChiGiaoHang = order.DiaChiGiaoHang,
                SoDienThoai = order.NguoiDung.SoDienThoai,
                PhuongThucThanhToan = "Thanh toán khi nhận hàng (COD)",
                TongTienThanhToan = order.TongTien,
                Items = order.ChiTietHoaDons.Select(item => new CartItemViewModel
                {
                    TenSanPham = item.MonAn?.TenMonAn ?? item.Combo?.TenCombo,
                    SoLuong = item.SoLuong,
                    DonGia = item.DonGia,
                    HinhAnh = Convert.ToBase64String(item.MonAn?.HinhAnh ?? item.Combo?.HinhAnh ?? new byte[0])
                }).ToList()
            };
            return View(viewModel);
        }
    }
}
