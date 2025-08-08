using FastFoodApp.Data;
using ASM1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FastFoodApp.Areas.Admin.Models;

namespace FastFoodApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CombosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CombosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // CẬP NHẬT LỚN: Action Index với Tìm kiếm, Lọc giá và Sắp xếp
        public async Task<IActionResult> Index(
            string sortOrder,
            string currentSearchTerm,
            string searchTerm,
            decimal? currentMinPrice,
            decimal? minPrice,
            decimal? currentMaxPrice,
            decimal? maxPrice,
            int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["PriceSortParm"] = sortOrder == "Price" ? "price_desc" : "Price";

            // Nếu có một bộ lọc mới được áp dụng, reset về trang 1
            if (searchTerm != null || minPrice != null || maxPrice != null)
            {
                pageNumber = 1;
            }
            else // Ngược lại, giữ lại các bộ lọc từ trang trước
            {
                searchTerm = currentSearchTerm;
                minPrice = currentMinPrice;
                maxPrice = currentMaxPrice;
            }

            // Lưu lại các giá trị lọc hiện tại
            ViewData["CurrentSearchTerm"] = searchTerm;
            ViewData["CurrentMinPrice"] = minPrice;
            ViewData["CurrentMaxPrice"] = maxPrice;

            var combosQuery = _context.Combos.AsQueryable();

            // Áp dụng tìm kiếm
            if (!String.IsNullOrEmpty(searchTerm))
            {
                combosQuery = combosQuery.Where(c => 
                    c.TenCombo.Contains(searchTerm) || 
                    c.MoTa.Contains(searchTerm) ||
                    c.MaCombo.ToString().Contains(searchTerm));
            }

            // Lọc theo giá
            if (minPrice.HasValue)
            {
                combosQuery = combosQuery.Where(c => c.Gia >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                combosQuery = combosQuery.Where(c => c.Gia <= maxPrice.Value);
            }

            // Áp dụng sắp xếp
            switch (sortOrder)
            {
                case "name_desc":
                    combosQuery = combosQuery.OrderByDescending(c => c.TenCombo);
                    break;
                case "Price":
                    combosQuery = combosQuery.OrderBy(c => c.Gia);
                    break;
                case "price_desc":
                    combosQuery = combosQuery.OrderByDescending(c => c.Gia);
                    break;
                default: // Mặc định sắp xếp theo tên
                    combosQuery = combosQuery.OrderBy(c => c.TenCombo);
                    break;
            }

            int pageSize = 10;
            var paginatedList = await PaginatedList<Combo>.CreateAsync(combosQuery.AsNoTracking(), pageNumber ?? 1, pageSize);

            return View(paginatedList);
        }

        // Action để tải form Thêm dưới dạng Partial View cho modal
        [HttpGet]
        public IActionResult CreatePartial()
        {
            return PartialView("_ComboForm", new Combo());
        }

        // Action để tải form Sửa dưới dạng Partial View cho modal
        [HttpGet]
        public async Task<IActionResult> EditPartial(int? id)
        {
            if (id == null) return NotFound();
            var combo = await _context.Combos.FindAsync(id);
            if (combo == null) return NotFound();
            return PartialView("_ComboForm", combo);
        }

        // Action xử lý việc Thêm từ modal
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TenCombo,MoTa,Gia")] Combo combo, IFormFile HinhAnhFile)
        {
            Console.WriteLine("=== DEBUG: CombosController.Create POST ===");
            Console.WriteLine($"ModelState.IsValid: {ModelState.IsValid}");
            Console.WriteLine($"TenCombo: {combo.TenCombo}");
            Console.WriteLine($"MoTa: {combo.MoTa}");
            Console.WriteLine($"Gia: {combo.Gia}");
            Console.WriteLine($"HinhAnhFile: {(HinhAnhFile != null ? $"Length: {HinhAnhFile.Length}, Name: {HinhAnhFile.FileName}" : "null")}");

            if (!ModelState.IsValid)
            {
                Console.WriteLine("ModelState Errors:");
                foreach (var error in ModelState)
                {
                    Console.WriteLine($"Key: {error.Key}, Errors: {string.Join(", ", error.Value.Errors.Select(e => e.ErrorMessage))}");
                }
            }

            ModelState.Remove("ChiTietCombos");
            
            Console.WriteLine($"ModelState.IsValid: {ModelState.IsValid}");

            if (ModelState.IsValid)
            {
                if (HinhAnhFile != null && HinhAnhFile.Length > 0)
                {
                    Console.WriteLine("Processing image file...");
                    
                    // Validate file type
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
                    var fileExtension = Path.GetExtension(HinhAnhFile.FileName).ToLowerInvariant();
                    
                    if (!allowedExtensions.Contains(fileExtension))
                    {
                        Console.WriteLine($"Invalid file extension: {fileExtension}");
                        ModelState.AddModelError("HinhAnhFile", "Chỉ chấp nhận file ảnh (.jpg, .jpeg, .png, .gif, .bmp)");
                        return PartialView("_ComboForm", combo);
                    }

                    // Validate file size (5MB limit)
                    if (HinhAnhFile.Length > 5 * 1024 * 1024)
                    {
                        Console.WriteLine($"File too large: {HinhAnhFile.Length} bytes");
                        ModelState.AddModelError("HinhAnhFile", "Kích thước file không được vượt quá 5MB");
                        return PartialView("_ComboForm", combo);
                    }

                    using (var memoryStream = new MemoryStream())
                    {
                        await HinhAnhFile.CopyToAsync(memoryStream);
                        combo.HinhAnh = memoryStream.ToArray();
                        Console.WriteLine($"Image saved successfully, size: {combo.HinhAnh.Length} bytes");
                    }
                }
                else
                {
                    Console.WriteLine("No image file provided");
                }

                try
                {
                    _context.Add(combo);
                    await _context.SaveChangesAsync();
                    Console.WriteLine("Combo saved successfully to database");
                    TempData["ToastSuccess"] = "Thêm combo thành công!";
                    return Json(new { success = true });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error saving combo: {ex.Message}");
                    ModelState.AddModelError("", "Có lỗi xảy ra khi lưu combo");
                }
            }
            
            Console.WriteLine("Returning PartialView with errors");
            // Nếu có lỗi, trả về form với thông báo lỗi
            return PartialView("_ComboForm", combo);
        }

        // Action xử lý việc Sửa từ modal
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaCombo,TenCombo,MoTa,Gia")] Combo combo, IFormFile HinhAnhFile)
        {
            Console.WriteLine("=== DEBUG: CombosController.Edit POST ===");
            Console.WriteLine($"id: {id}, combo.MaCombo: {combo.MaCombo}");
            Console.WriteLine($"TenCombo: '{combo.TenCombo}'");
            Console.WriteLine($"MoTa: '{combo.MoTa}'");
            Console.WriteLine($"Gia: {combo.Gia}");
            Console.WriteLine($"HinhAnhFile: {(HinhAnhFile != null ? $"Length: {HinhAnhFile.Length}, Name: {HinhAnhFile.FileName}" : "null")}");

            if (id != combo.MaCombo) 
            {
                Console.WriteLine($"ID mismatch: URL id={id}, Model id={combo.MaCombo}");
                return NotFound();
            }

            // Loại bỏ lỗi validation cho ChiTietCombos navigation property
            ModelState.Remove("ChiTietCombos");
            
            Console.WriteLine($"ModelState.IsValid: {ModelState.IsValid}");

            if (!ModelState.IsValid)
            {
                Console.WriteLine("ModelState Errors:");
                foreach (var error in ModelState)
                {
                    Console.WriteLine($"Key: {error.Key}, Errors: {string.Join(", ", error.Value.Errors.Select(e => e.ErrorMessage))}");
                }
                
                // Load existing image for display in the form
                var existingCombo = await _context.Combos.AsNoTracking().FirstOrDefaultAsync(c => c.MaCombo == id);
                if (existingCombo != null)
                {
                    combo.HinhAnh = existingCombo.HinhAnh;
                }
                
                Console.WriteLine("Returning PartialView with ModelState errors");
                return PartialView("_ComboForm", combo);
            }

            try
            {
                // IMPORTANT: Always load the existing record first to avoid losing data
                var existingCombo = await _context.Combos.AsNoTracking().FirstOrDefaultAsync(c => c.MaCombo == id);
                if (existingCombo == null)
                {
                    return NotFound();
                }
                
                // Keep existing image by default
                combo.HinhAnh = existingCombo.HinhAnh;
                
                // Only process new image if one was uploaded
                if (HinhAnhFile != null && HinhAnhFile.Length > 0)
                {
                    Console.WriteLine("Processing new image file...");
                    
                    // Validate file type
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
                    var fileExtension = Path.GetExtension(HinhAnhFile.FileName).ToLowerInvariant();
                    
                    if (!allowedExtensions.Contains(fileExtension))
                    {
                        Console.WriteLine($"Invalid file extension: {fileExtension}");
                        ModelState.AddModelError("HinhAnhFile", "Chỉ chấp nhận file ảnh (.jpg, .jpeg, .png, .gif, .bmp)");
                        return PartialView("_ComboForm", combo);
                    }

                    // Validate file size (5MB limit)
                    if (HinhAnhFile.Length > 5 * 1024 * 1024)
                    {
                        Console.WriteLine($"File too large: {HinhAnhFile.Length} bytes");
                        ModelState.AddModelError("HinhAnhFile", "Kích thước file không được vượt quá 5MB");
                        return PartialView("_ComboForm", combo);
                    }

                    using (var memoryStream = new MemoryStream())
                    {
                        await HinhAnhFile.CopyToAsync(memoryStream);
                        combo.HinhAnh = memoryStream.ToArray();
                        Console.WriteLine($"New image saved successfully, size: {combo.HinhAnh.Length} bytes");
                    }
                }
                else
                {
                    Console.WriteLine("No new image file, keeping existing image...");
                    Console.WriteLine("Existing image preserved");
                }

                _context.Update(combo);
                await _context.SaveChangesAsync();
                Console.WriteLine("Combo updated successfully in database");
                TempData["ToastSuccess"] = "Cập nhật combo thành công!";
                return Json(new { success = true, message = "Cập nhật thành công!" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating combo: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                ModelState.AddModelError("", "Có lỗi xảy ra khi cập nhật combo: " + ex.Message);
                return PartialView("_ComboForm", combo);
            }
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var combo = await _context.Combos.FirstOrDefaultAsync(m => m.MaCombo == id);
            if (combo == null) return NotFound();
            return View(combo);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var combo = await _context.Combos.FindAsync(id);
            if (combo != null)
            {
                _context.Combos.Remove(combo);
                await _context.SaveChangesAsync();
                TempData["ToastSuccess"] = "Đã xóa combo.";
            }
            else
            {
                TempData["ToastError"] = "Không tìm thấy combo.";
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/Combos/ManageComboItems/5
        public async Task<IActionResult> ManageComboItems(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var combo = await _context.Combos
                .FirstOrDefaultAsync(m => m.MaCombo == id);
                
            if (combo == null)
            {
                return NotFound();
            }

            // Lấy danh sách sản phẩm trong combo
            var comboProducts = await _context.ChiTietCombos
                .Where(c => c.MaCombo == id)
                .Select(c => c.MonAn)
                .ToListAsync();

            // Lấy tất cả sản phẩm
            var allProducts = await _context.MonAns
                .Include(p => p.LoaiMonAn)
                .ToListAsync();

            ViewBag.ComboProducts = comboProducts;
            ViewBag.AllProducts = allProducts;

            return View(combo);
        }

        // POST: Admin/Combos/AddProductToCombo
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProductToCombo(int comboId, int productId)
        {
            // Kiểm tra combo và sản phẩm tồn tại
            var combo = await _context.Combos.FindAsync(comboId);
            var product = await _context.MonAns.FindAsync(productId);

            if (combo == null || product == null)
            {
                TempData["ToastError"] = "Không tìm thấy combo hoặc sản phẩm.";
                return RedirectToAction(nameof(Index));
            }

            // Kiểm tra sản phẩm đã có trong combo chưa
            var existingItem = await _context.ChiTietCombos
                .FirstOrDefaultAsync(c => c.MaCombo == comboId && c.MaMonAn == productId);

            if (existingItem != null)
            {
                TempData["ToastError"] = "Sản phẩm đã có trong combo.";
                return RedirectToAction(nameof(ManageComboItems), new { id = comboId });
            }

            // Thêm sản phẩm vào combo
            var comboItem = new ChiTietCombo
            {
                MaCombo = comboId,
                MaMonAn = productId
            };

            _context.ChiTietCombos.Add(comboItem);
            await _context.SaveChangesAsync();

            TempData["ToastSuccess"] = $"Đã thêm {product.TenMonAn} vào combo.";
            return RedirectToAction(nameof(ManageComboItems), new { id = comboId });
        }

        // POST: Admin/Combos/RemoveProductFromCombo
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveProductFromCombo(int comboId, int productId)
        {
            var comboItem = await _context.ChiTietCombos
                .FirstOrDefaultAsync(c => c.MaCombo == comboId && c.MaMonAn == productId);

            if (comboItem == null)
            {
                TempData["ToastError"] = "Không tìm thấy sản phẩm trong combo.";
                return RedirectToAction(nameof(ManageComboItems), new { id = comboId });
            }

            var productName = await _context.MonAns
                .Where(p => p.MaMonAn == productId)
                .Select(p => p.TenMonAn)
                .FirstOrDefaultAsync();

            _context.ChiTietCombos.Remove(comboItem);
            await _context.SaveChangesAsync();

            TempData["ToastSuccess"] = $"Đã xóa {productName} khỏi combo.";
            return RedirectToAction(nameof(ManageComboItems), new { id = comboId });
        }
    }
}
