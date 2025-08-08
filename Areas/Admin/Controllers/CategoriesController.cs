// File: Areas/Admin/Controllers/CategoriesController.cs
using FastFoodApp.Data;
using ASM1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace FastFoodApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Action Index để hiển thị danh sách
        public async Task<IActionResult> Index()
        {
            return View(await _context.LoaiMonAns.ToListAsync());
        }

        // Action để tải form Thêm dưới dạng Partial View cho modal
        [HttpGet]
        public IActionResult CreatePartial()
        {
            return PartialView("_CategoryForm", new LoaiMonAn());
        }

        // Action để tải form Sửa dưới dạng Partial View cho modal
        [HttpGet]
        public async Task<IActionResult> EditPartial(int? id)
        {
            if (id == null) return NotFound();
            var loaiMonAn = await _context.LoaiMonAns.FindAsync(id);
            if (loaiMonAn == null) return NotFound();
            return PartialView("_CategoryForm", loaiMonAn);
        }

        // Action xử lý việc Thêm từ modal
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TenLoai")] LoaiMonAn loaiMonAn)
        {
            if (ModelState.IsValid)
            {
                _context.Add(loaiMonAn);
                await _context.SaveChangesAsync();
                TempData["ToastSuccess"] = "Thêm danh mục thành công!";
                return Json(new { success = true });
            }
            // Nếu có lỗi, trả về form với thông báo lỗi
            return PartialView("_CategoryForm", loaiMonAn);
        }

        // Action xử lý việc Sửa từ modal
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaLoai,TenLoai")] LoaiMonAn loaiMonAn)
        {
            if (id != loaiMonAn.MaLoai) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(loaiMonAn);
                await _context.SaveChangesAsync();
                TempData["ToastSuccess"] = "Cập nhật danh mục thành công!";
                return Json(new { success = true });
            }
            // Nếu có lỗi, trả về form với thông báo lỗi
            return PartialView("_CategoryForm", loaiMonAn);
        }

        // Action Delete và DeleteConfirmed giữ nguyên luồng chuyển trang
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var loaiMonAn = await _context.LoaiMonAns.FirstOrDefaultAsync(m => m.MaLoai == id);
            if (loaiMonAn == null) return NotFound();
            return View(loaiMonAn);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var loaiMonAn = await _context.LoaiMonAns.FindAsync(id);
            _context.LoaiMonAns.Remove(loaiMonAn);
            await _context.SaveChangesAsync();
            TempData["ToastSuccess"] = "Đã xóa danh mục.";
            return RedirectToAction(nameof(Index));
        }
    }
}
