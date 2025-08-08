using ASM1.Models;
using FastFoodApp.Data;
using FastFoodApp.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace FastFoodApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _context.NguoiDungs.FirstOrDefaultAsync(u => u.Email == model.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError(string.Empty, "Địa chỉ email này đã được sử dụng.");
                    return View(model);
                }

                var user = new NguoiDung
                {
                    HoTen = model.HoTen,
                    Email = model.Email,
                    MatKhau = BCrypt.Net.BCrypt.HashPassword(model.MatKhau),
                    SoDienThoai = model.SoDienThoai,
                    NgaySinh = model.NgaySinh,
                    MaVaiTro = 2,
                    MaCapThanhVien = 1,
                    DiemTichLuy = 0,
                    DiaChis = new List<DiaChiNguoiDung>
                    {
                        new DiaChiNguoiDung
                        {
                            TenNguoiNhan = model.HoTen,
                            DiaChiCuThe = model.DiaChi,
                            SoDienThoai = model.SoDienThoai,
                            IsDefault = true
                        }
                    }
                };
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction("Login", "Account");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.NguoiDungs
                                         .Include(u => u.VaiTro) 
                                         .FirstOrDefaultAsync(u => u.Email == model.Email);

                if (user != null && !string.IsNullOrEmpty(user.MatKhau) && BCrypt.Net.BCrypt.Verify(model.MatKhau, user.MatKhau))
                {
                    await SignInUser(user);

                    if (user.VaiTro?.TenVaiTro == "Admin")
                    {
                        TempData["ToastSuccess"] = $"Chào mừng quản trị viên {user.HoTen}!";
                        return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
                    }

                    TempData["ToastSuccess"] = $"Chào mừng {user.HoTen}!";
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError(string.Empty, "Email hoặc mật khẩu không chính xác.");
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            TempData["ToastInfo"] = "Bạn đã đăng xuất.";
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            // SỬA LỖI: Kiểm tra giá trị UserId trước khi Parse
            var userIdString = User.FindFirstValue("UserId");
            if (string.IsNullOrEmpty(userIdString))
            {
                // Nếu không tìm thấy UserId, đăng xuất người dùng và chuyển hướng về trang đăng nhập
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                TempData["ToastError"] = "Phiên đăng nhập không hợp lệ, vui lòng đăng nhập lại.";
                return RedirectToAction("Login");
            }

            var userId = int.Parse(userIdString);
            var viewModel = await BuildProfileViewModel(userId);

            if (viewModel == null)
            {
                // Trường hợp user đã bị xóa khỏi DB nhưng cookie vẫn còn
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                TempData["ToastError"] = "Không tìm thấy thông tin người dùng.";
                return RedirectToAction("Login");
            }

            return View(viewModel);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(ProfileViewModel model)
        {
            var userId = int.Parse(User.FindFirstValue("UserId"));

            if (!ModelState.IsValid)
            {
                var fullViewModel = await BuildProfileViewModel(userId);
                
                if (fullViewModel != null)
                {
                    fullViewModel.HoTen = model.HoTen;
                    fullViewModel.SoDienThoai = model.SoDienThoai;
                    fullViewModel.NgaySinh = model.NgaySinh;
                    return View(fullViewModel);
                }
                
                // If there's an issue with building the full view model, fallback to returning the original model
                return View(model);
            }

            var user = await _context.NguoiDungs.FindAsync(userId);
            if (user == null) return NotFound();

            user.HoTen = model.HoTen;
            user.SoDienThoai = model.SoDienThoai;
            user.NgaySinh = model.NgaySinh;

            _context.Update(user);
            await _context.SaveChangesAsync();
            TempData["ToastSuccess"] = "Cập nhật thông tin thành công!";

            return RedirectToAction(nameof(Profile));
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddAddress([Bind(Prefix = "NewAddress")] DiaChiNguoiDung newAddress)
        {
            var userIdString = User.FindFirstValue("UserId");
            if (string.IsNullOrEmpty(userIdString))
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                TempData["ToastError"] = "Phiên đăng nhập không hợp lệ, vui lòng đăng nhập lại.";
                return RedirectToAction("Login");
            }

            var userId = int.Parse(userIdString);
            newAddress.MaNguoiDung = userId;

            ModelState.Clear();
            TryValidateModel(newAddress);

            if (ModelState.IsValid)
            {
                var userAddresses = await _context.DiaChiNguoiDungs.Where(a => a.MaNguoiDung == userId).ToListAsync();
                if (!userAddresses.Any())
                {
                    newAddress.IsDefault = true;
                }

                _context.DiaChiNguoiDungs.Add(newAddress);
                await _context.SaveChangesAsync();
                TempData["ToastSuccess"] = "Thêm địa chỉ mới thành công!";
            }
            else
            {
                TempData["ToastError"] = "Thêm địa chỉ thất bại. Vui lòng kiểm tra lại thông tin.";
            }

            return RedirectToAction("Profile");
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAddress(int id)
        {
            var userIdString = User.FindFirstValue("UserId");
            if (string.IsNullOrEmpty(userIdString))
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                TempData["ToastError"] = "Phiên đăng nhập không hợp lệ, vui lòng đăng nhập lại.";
                return RedirectToAction("Login");
            }

            var userId = int.Parse(userIdString);
            var address = await _context.DiaChiNguoiDungs.FirstOrDefaultAsync(a => a.MaDiaChi == id && a.MaNguoiDung == userId);

            if (address != null)
            {
                if (address.IsDefault)
                {
                    TempData["ToastWarning"] = "Không thể xóa địa chỉ mặc định.";
                }
                else
                {
                    _context.DiaChiNguoiDungs.Remove(address);
                    await _context.SaveChangesAsync();
                    TempData["ToastSuccess"] = "Đã xóa địa chỉ.";
                }
            }
            return RedirectToAction("Profile");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetDefaultAddress(int id)
        {
            var userIdString = User.FindFirstValue("UserId");
            if (string.IsNullOrEmpty(userIdString))
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                TempData["ToastError"] = "Phiên đăng nhập không hợp lệ, vui lòng đăng nhập lại.";
                return RedirectToAction("Login");
            }

            var userId = int.Parse(userIdString);
            var userAddresses = await _context.DiaChiNguoiDungs
                                              .Where(a => a.MaNguoiDung == userId)
                                              .ToListAsync();

            if (!userAddresses.Any(a => a.MaDiaChi == id))
            {
                return NotFound();
            }

            foreach (var addr in userAddresses)
            {
                addr.IsDefault = (addr.MaDiaChi == id);
            }

            await _context.SaveChangesAsync();
            TempData["ToastSuccess"] = "Đã cập nhật địa chỉ mặc định.";
            return RedirectToAction("Profile");
        }

        [AllowAnonymous]
        public IActionResult ExternalLogin(string provider, string? returnUrl = null)
        {
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account", new { returnUrl });
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, provider);
        }

        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string? returnUrl = null, string? remoteError = null)
        {
            if (remoteError != null)
            {
                TempData["ToastError"] = $"Lỗi từ nhà cung cấp ngoài: {remoteError}";
                return RedirectToAction(nameof(Login));
            }

            var info = await HttpContext.AuthenticateAsync("Google");
            if (info?.Principal == null)
            {
                return RedirectToAction(nameof(Login));
            }

            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            var name = info.Principal.FindFirstValue(ClaimTypes.Name) ?? "Người dùng Google";

            if (email == null)
            {
                TempData["ToastError"] = "Không thể lấy thông tin email từ Google.";
                return RedirectToAction(nameof(Login));
            }

            var user = await _context.NguoiDungs.Include(u => u.VaiTro).FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                user = new NguoiDung
                {
                    Email = email,
                    HoTen = name,
                    MaVaiTro = 2,
                    MaCapThanhVien = 1,
                    DiemTichLuy = 0,
                    DiaChis = new List<DiaChiNguoiDung>
                    {
                        new DiaChiNguoiDung
                        {
                            TenNguoiNhan = name,
                            SoDienThoai = "Chưa cập nhật",
                            DiaChiCuThe = "Chưa cập nhật",
                            IsDefault = true
                        }
                    }
                };
                _context.NguoiDungs.Add(user);
                await _context.SaveChangesAsync();
                user = await _context.NguoiDungs.Include(u => u.VaiTro).FirstAsync(u => u.Email == email);
            }

            await SignInUser(user);
            TempData["ToastSuccess"] = $"Chào mừng {user.HoTen}!";
            return RedirectToAction("Index", "Home");
        }

        private async Task SignInUser(NguoiDung user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.HoTen),
                new Claim("UserId", user.MaNguoiDung.ToString()),
                new Claim(ClaimTypes.Role, user.VaiTro.TenVaiTro)
            };
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties { IsPersistent = true };
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
        }

        [Authorize]
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userIdString = User.FindFirstValue("UserId");
            if (string.IsNullOrEmpty(userIdString))
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                TempData["ToastError"] = "Phiên đăng nhập không hợp lệ, vui lòng đăng nhập lại.";
                return RedirectToAction("Login");
            }

            var userId = int.Parse(userIdString);
            var user = await _context.NguoiDungs.FindAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            // Check if the user account was created with external login (Google)
            if (string.IsNullOrEmpty(user.MatKhau))
            {
                ModelState.AddModelError(string.Empty, "Tài khoản của bạn được tạo bằng đăng nhập Google và không có mật khẩu để thay đổi.");
                return View(model);
            }

            // Verify current password
            if (!BCrypt.Net.BCrypt.Verify(model.MatKhauHienTai, user.MatKhau))
            {
                ModelState.AddModelError("MatKhauHienTai", "Mật khẩu hiện tại không chính xác.");
                return View(model);
            }

            // Update password
            user.MatKhau = BCrypt.Net.BCrypt.HashPassword(model.MatKhauMoi);
            _context.Update(user);
            await _context.SaveChangesAsync();

            TempData["ToastSuccess"] = "Mật khẩu đã được thay đổi thành công!";
            return RedirectToAction(nameof(Profile));
        }

        private async Task<ProfileViewModel?> BuildProfileViewModel(int userId)
        {
            var user = await _context.NguoiDungs
                                     .Include(u => u.CapThanhVien)
                                     .Include(u => u.DiaChis)
                                     .AsNoTracking()
                                     .FirstOrDefaultAsync(u => u.MaNguoiDung == userId);

            if (user == null) return null;

            var totalOrders = await _context.HoaDons.CountAsync(h => h.MaNguoiDung == userId);
            var completedOrders = await _context.HoaDons.CountAsync(h => h.MaNguoiDung == userId && h.TrangThai == "Đã giao");
            var totalSpent = await _context.HoaDons.Where(h => h.MaNguoiDung == userId && h.TrangThai == "Đã giao").SumAsync(h => (decimal?)h.TongTien) ?? 0;
            var allMemberships = await _context.CapThanhViens.OrderBy(c => c.NguongDiem).ToListAsync();

            CapThanhVien? nextMembership = null;
            double progress = 0;

            if (user.CapThanhVien != null)
            {
                nextMembership = allMemberships.FirstOrDefault(m => m.NguongDiem > user.CapThanhVien.NguongDiem);
                if (nextMembership != null)
                {
                    var pointsForNextLevel = nextMembership.NguongDiem - user.CapThanhVien.NguongDiem;
                    var userProgressPoints = user.DiemTichLuy - user.CapThanhVien.NguongDiem;
                    progress = pointsForNextLevel > 0 ? (double)userProgressPoints / pointsForNextLevel * 100 : 100;
                }
            }

            return new ProfileViewModel
            {
                MaNguoiDung = user.MaNguoiDung,
                Email = user.Email ?? string.Empty,
                HoTen = user.HoTen,
                SoDienThoai = user.SoDienThoai,
                NgaySinh = user.NgaySinh,
                TotalOrders = totalOrders,
                CompletedOrders = completedOrders,
                TotalSpent = totalSpent,
                CurrentMembership = user.CapThanhVien,
                AllMemberships = allMemberships,
                NextMembership = nextMembership,
                ProgressToNextLevel = Math.Max(0, Math.Min(100, progress)),
                Addresses = user.DiaChis.ToList(),
                NewAddress = new DiaChiNguoiDung()
            };
        }
    }

    public class AddressPayload
    {
        public string Address { get; set; } = string.Empty;
    }
}
