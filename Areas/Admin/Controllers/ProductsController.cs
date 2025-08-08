using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ASM1.Models;
using FastFoodApp.Data;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using FastFoodApp.Areas.Admin.Models;

namespace FastFoodApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductsController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Admin/Products
        public async Task<IActionResult> Index(
            string sortOrder,
            string searchTerm,
            string categoryFilter,
            decimal? minPrice,
            decimal? maxPrice,
            int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = sortOrder == "name_asc" ? "name_desc" : "name_asc";
            ViewData["PriceSortParm"] = sortOrder == "price_asc" ? "price_desc" : "price_asc";
            ViewData["CategorySortParm"] = sortOrder == "category_asc" ? "category_desc" : "category_asc";
            
            ViewData["CurrentSearchTerm"] = searchTerm;
            ViewData["CurrentCategoryFilter"] = categoryFilter;
            ViewData["CurrentMinPrice"] = minPrice;
            ViewData["CurrentMaxPrice"] = maxPrice;

            var query = _context.MonAns
                .Include(m => m.LoaiMonAn)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(p => p.TenMonAn.Contains(searchTerm) || 
                                        p.MoTa.Contains(searchTerm));
            }

            if (!string.IsNullOrEmpty(categoryFilter))
            {
                query = query.Where(p => p.LoaiMonAn.TenLoai == categoryFilter);
            }

            if (minPrice.HasValue)
            {
                query = query.Where(p => p.Gia >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(p => p.Gia <= maxPrice.Value);
            }

            query = sortOrder switch
            {
                "name_asc" => query.OrderBy(p => p.TenMonAn),
                "name_desc" => query.OrderByDescending(p => p.TenMonAn),
                "price_asc" => query.OrderBy(p => p.Gia),
                "price_desc" => query.OrderByDescending(p => p.Gia),
                "category_asc" => query.OrderBy(p => p.LoaiMonAn.TenLoai),
                "category_desc" => query.OrderByDescending(p => p.LoaiMonAn.TenLoai),
                _ => query.OrderBy(p => p.MaMonAn),
            };

            var categories = await _context.LoaiMonAns
                .Select(c => c.TenLoai)
                .Distinct()
                .ToListAsync();
            ViewBag.Categories = categories;

            int pageSize = 10;
            // Dòng này cần model PaginatedList của bạn, đảm bảo nó tồn tại và đúng
            return View(await PaginatedList<MonAn>.CreateAsync(query, pageNumber ?? 1, pageSize));
        }

        // GET: Admin/Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var product = await _context.MonAns
                .Include(p => p.LoaiMonAn)
                .FirstOrDefaultAsync(m => m.MaMonAn == id);
            if (product == null) return NotFound();
            return View(product);
        }

        // ===================================================================
        // START: CÁC ACTION ĐƯỢC THÊM MỚI VÀ SỬA ĐỔI CHO MODAL AJAX
        // ===================================================================

        // GET: Admin/Products/CreatePartial
        // Action này trả về form HTML để hiển thị trong modal
        [HttpGet]
        public async Task<IActionResult> CreatePartial()
        {
            ViewBag.MaLoai = new SelectList(await _context.LoaiMonAns.ToListAsync(), "MaLoai", "TenLoai");
            return PartialView("_ProductForm", new ProductViewModel());
        }

        // POST: Admin/Products/Create
        // Action này xử lý việc tạo mới sản phẩm từ modal
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var product = new MonAn
                {
                    TenMonAn = viewModel.TenMonAn,
                    MoTa = viewModel.MoTa,
                    Gia = viewModel.Gia,
                    MaLoai = viewModel.MaLoai
                };

                if (viewModel.HinhAnhFile != null && viewModel.HinhAnhFile.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await viewModel.HinhAnhFile.CopyToAsync(memoryStream);
                        product.HinhAnh = memoryStream.ToArray();
                    }
                }
                
                _context.Add(product);
                await _context.SaveChangesAsync();
                TempData["ToastSuccess"] = "Thêm sản phẩm thành công!";
                return Json(new { success = true });
            }
            
            ViewBag.MaLoai = new SelectList(await _context.LoaiMonAns.ToListAsync(), "MaLoai", "TenLoai", viewModel.MaLoai);
            return PartialView("_ProductForm", viewModel);
        }

        // GET: Admin/Products/EditPartial/5
        // Action này lấy thông tin sản phẩm và trả về form HTML để sửa trong modal
        [HttpGet]
        public async Task<IActionResult> EditPartial(int? id)
        {
            if (id == null) return NotFound();

            var product = await _context.MonAns.FindAsync(id);
            if (product == null) return NotFound();

            var viewModel = new ProductViewModel
            {
                MaMonAn = product.MaMonAn,
                TenMonAn = product.TenMonAn,
                MoTa = product.MoTa,
                Gia = product.Gia,
                MaLoai = product.MaLoai,
                HinhAnh = product.HinhAnh
            };
            
            ViewBag.MaLoai = new SelectList(await _context.LoaiMonAns.ToListAsync(), "MaLoai", "TenLoai", product.MaLoai);
            return PartialView("_ProductForm", viewModel);
        }

        // POST: Admin/Products/Edit/5
        // Action này xử lý việc cập nhật sản phẩm từ modal
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductViewModel viewModel)
        {
            if (id != viewModel.MaMonAn) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var product = await _context.MonAns.FindAsync(id);
                    if (product == null) return NotFound();

                    product.TenMonAn = viewModel.TenMonAn;
                    product.MoTa = viewModel.MoTa;
                    product.Gia = viewModel.Gia;
                    product.MaLoai = viewModel.MaLoai;

                    if (viewModel.HinhAnhFile != null && viewModel.HinhAnhFile.Length > 0)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await viewModel.HinhAnhFile.CopyToAsync(memoryStream);
                            product.HinhAnh = memoryStream.ToArray();
                        }
                    }

                    _context.Update(product);
                    await _context.SaveChangesAsync();
                    TempData["ToastSuccess"] = "Cập nhật sản phẩm thành công!";
                    return Json(new { success = true });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(viewModel.MaMonAn)) return NotFound();
                    else throw;
                }
            }
            
            ViewBag.MaLoai = new SelectList(await _context.LoaiMonAns.ToListAsync(), "MaLoai", "TenLoai", viewModel.MaLoai);
            return PartialView("_ProductForm", viewModel);
        }
        
        // ===================================================================
        // END: CÁC ACTION ĐƯỢC THÊM MỚI VÀ SỬA ĐỔI
        // ===================================================================

        // GET: Admin/Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var product = await _context.MonAns.FindAsync(id);
            if (product == null) return NotFound();

            ViewBag.MaLoai = new SelectList(await _context.LoaiMonAns.ToListAsync(), "MaLoai", "TenLoai", product.MaLoai);
            return View(product);
        }

        // POST: Admin/Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int id, MonAn product, IFormFile HinhAnhFile)
        {
            if (id != product.MaMonAn) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    if (HinhAnhFile != null && HinhAnhFile.Length > 0)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await HinhAnhFile.CopyToAsync(memoryStream);
                            product.HinhAnh = memoryStream.ToArray();
                        }
                    }
                    else
                    {
                        // Giữ lại hình ảnh cũ nếu không có file mới được tải lên
                        var existingProduct = await _context.MonAns.AsNoTracking().FirstOrDefaultAsync(p => p.MaMonAn == id);
                        if (existingProduct != null)
                        {
                            product.HinhAnh = existingProduct.HinhAnh;
                        }
                    }

                    _context.Update(product);
                    await _context.SaveChangesAsync();
                    TempData["ToastSuccess"] = "Cập nhật sản phẩm thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.MaMonAn)) return NotFound();
                    else throw;
                }
            }
            
            // Nếu model không hợp lệ, trả về lại form với lỗi
            ViewBag.MaLoai = new SelectList(await _context.LoaiMonAns.ToListAsync(), "MaLoai", "TenLoai", product.MaLoai);
            return View("Edit", product);
        }

        // GET: Admin/Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var product = await _context.MonAns
                .Include(p => p.LoaiMonAn)
                .FirstOrDefaultAsync(m => m.MaMonAn == id);
            if (product == null) return NotFound();
            return View(product);
        }

        // POST: Admin/Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.MonAns.FindAsync(id);
            if (product != null)
            {
                _context.MonAns.Remove(product);
                await _context.SaveChangesAsync();
                TempData["ToastSuccess"] = "Xóa sản phẩm thành công!";
            }
            return RedirectToAction(nameof(Index));
        }
        
        private bool ProductExists(int id)
        {
            return _context.MonAns.Any(e => e.MaMonAn == id);
        }
    }
}
