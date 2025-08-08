// File: Areas/Admin/Controllers/UsersController.cs
using FastFoodApp.Data;
using ASM1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FastFoodApp.Areas.Admin.Models;

namespace FastFoodApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // CẬP NHẬT LỚN: Action Index với Tìm kiếm, Lọc vai trò, Lọc ngày và Sắp xếp
        public async Task<IActionResult> Index(
            string sortOrder,
            string currentSearchTerm,
            string searchTerm,
            string currentRoleFilter,
            string roleFilter,
            DateTime? currentStartDate,
            DateTime? startDate,
            DateTime? currentEndDate,
            DateTime? endDate,
            int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["EmailSortParm"] = sortOrder == "Email" ? "email_desc" : "Email";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";

            // Nếu có một bộ lọc mới được áp dụng, reset về trang 1
            if (searchTerm != null || roleFilter != null || startDate != null || endDate != null)
            {
                pageNumber = 1;
            }
            else // Ngược lại, giữ lại các bộ lọc từ trang trước
            {
                searchTerm = currentSearchTerm;
                roleFilter = currentRoleFilter;
                startDate = currentStartDate;
                endDate = currentEndDate;
            }

            // Lưu lại các giá trị lọc hiện tại
            ViewData["CurrentSearchTerm"] = searchTerm;
            ViewData["CurrentRoleFilter"] = roleFilter;
            ViewData["CurrentStartDate"] = startDate?.ToString("yyyy-MM-dd");
            ViewData["CurrentEndDate"] = endDate?.ToString("yyyy-MM-dd");

            var usersQuery = _context.NguoiDungs.Include(u => u.VaiTro).AsQueryable();

            // Áp dụng tìm kiếm
            if (!String.IsNullOrEmpty(searchTerm))
            {
                usersQuery = usersQuery.Where(u => 
                    u.HoTen.Contains(searchTerm) || 
                    u.Email.Contains(searchTerm) || 
                    u.SoDienThoai.Contains(searchTerm) ||
                    u.MaNguoiDung.ToString().Contains(searchTerm));
            }

            // Lọc theo vai trò
            if (!String.IsNullOrEmpty(roleFilter))
            {
                usersQuery = usersQuery.Where(u => u.VaiTro.TenVaiTro == roleFilter);
            }

            // Lọc theo ngày tạo
            if (startDate.HasValue)
            {
                usersQuery = usersQuery.Where(u => u.NgaySinh >= startDate.Value.Date);
            }

            if (endDate.HasValue)
            {
                usersQuery = usersQuery.Where(u => u.NgaySinh <= endDate.Value.Date);
            }

            // Áp dụng sắp xếp
            switch (sortOrder)
            {
                case "name_desc":
                    usersQuery = usersQuery.OrderByDescending(u => u.HoTen);
                    break;
                case "Email":
                    usersQuery = usersQuery.OrderBy(u => u.Email);
                    break;
                case "email_desc":
                    usersQuery = usersQuery.OrderByDescending(u => u.Email);
                    break;
                case "Date":
                    usersQuery = usersQuery.OrderBy(u => u.NgaySinh);
                    break;
                case "date_desc":
                    usersQuery = usersQuery.OrderByDescending(u => u.NgaySinh);
                    break;
                default: // Mặc định sắp xếp theo tên
                    usersQuery = usersQuery.OrderBy(u => u.HoTen);
                    break;
            }

            int pageSize = 10;
            var paginatedList = await PaginatedList<NguoiDung>.CreateAsync(usersQuery.AsNoTracking(), pageNumber ?? 1, pageSize);

            // Chuẩn bị danh sách vai trò cho dropdown
            ViewBag.RoleList = await _context.VaiTros.Select(r => r.TenVaiTro).Distinct().ToListAsync();

            return View(paginatedList);
        }

        private void PopulateRolesDropDownList()
        {
            ViewBag.MaVaiTro = new SelectList(_context.VaiTros, "MaVaiTro", "TenVaiTro");
        }

        [HttpGet]
        public IActionResult CreatePartial()
        {
            PopulateRolesDropDownList();
            return PartialView("_UserForm", new NguoiDung());
        }

        [HttpGet]
        public async Task<IActionResult> EditPartial(int? id)
        {
            if (id == null) return NotFound();
            var user = await _context.NguoiDungs.FindAsync(id);
            if (user == null) return NotFound();
            PopulateRolesDropDownList();
            return PartialView("_UserForm", user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HoTen,Email,MatKhau,SoDienThoai,NgaySinh,MaVaiTro")] NguoiDung nguoiDung)
        {
            if (ModelState.IsValid)
            {
                nguoiDung.MatKhau = BCrypt.Net.BCrypt.HashPassword(nguoiDung.MatKhau);
                _context.Add(nguoiDung);
                await _context.SaveChangesAsync();
                TempData["ToastSuccess"] = "Tạo người dùng thành công!";
                return Json(new { success = true });
            }
            PopulateRolesDropDownList();
            return PartialView("_UserForm", nguoiDung);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaNguoiDung,HoTen,Email,SoDienThoai,NgaySinh,MaVaiTro")] NguoiDung nguoiDung)
        {
            if (id != nguoiDung.MaNguoiDung) return NotFound();
            ModelState.Remove("MatKhau"); // Không validate mật khẩu khi sửa

            if (ModelState.IsValid)
            {
                var userFromDb = await _context.NguoiDungs.AsNoTracking().FirstOrDefaultAsync(u => u.MaNguoiDung == id);
                nguoiDung.MatKhau = userFromDb.MatKhau; // Giữ lại mật khẩu cũ

                _context.Update(nguoiDung);
                await _context.SaveChangesAsync();
                TempData["ToastSuccess"] = "Cập nhật thông tin thành công!";
                return Json(new { success = true });
            }
            PopulateRolesDropDownList();
            return PartialView("_UserForm", nguoiDung);
        }

        // Action Delete và Details giữ nguyên
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var user = await _context.NguoiDungs.Include(u => u.VaiTro).FirstOrDefaultAsync(m => m.MaNguoiDung == id);
            if (user == null) return NotFound();
            return View(user);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var user = await _context.NguoiDungs.FirstOrDefaultAsync(m => m.MaNguoiDung == id);
            if (user == null) return NotFound();
            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var currentUserId = User.FindFirstValue("UserId");
            if (id.ToString() == currentUserId)
            {
                TempData["ToastError"] = "Bạn không thể tự xóa tài khoản của mình.";
                return RedirectToAction(nameof(Index));
            }

            var user = await _context.NguoiDungs.FindAsync(id);
            _context.NguoiDungs.Remove(user);
            await _context.SaveChangesAsync();
            TempData["ToastSuccess"] = "Đã xóa người dùng.";
            return RedirectToAction(nameof(Index));
        }
    }
}
