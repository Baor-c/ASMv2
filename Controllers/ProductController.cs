using FastFoodApp.Data;
using ASM1.Models;
using FastFoodApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace FastFoodApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ĐÃ CẬP NHẬT: Tích hợp Tìm kiếm nâng cao, Phân trang và AsNoTracking
        public async Task<IActionResult> Index(string searchTerm, int? categoryId, decimal? minPrice, decimal? maxPrice, int page = 1)
        {
            var pageSize = 8;
            var productsQuery = _context.MonAns
                                        .AsNoTracking() // Tối ưu hiệu năng
                                        .Include(p => p.LoaiMonAn)
                                        .AsQueryable();

            // Tìm kiếm nâng cao
            if (!string.IsNullOrEmpty(searchTerm))
            {
                productsQuery = productsQuery.Where(p => p.TenMonAn.Contains(searchTerm) || p.MoTa.Contains(searchTerm));
            }
            if (categoryId.HasValue)
            {
                productsQuery = productsQuery.Where(p => p.MaLoai == categoryId.Value);
            }
            if (minPrice.HasValue)
            {
                productsQuery = productsQuery.Where(p => p.Gia >= minPrice.Value);
            }
            if (maxPrice.HasValue)
            {
                productsQuery = productsQuery.Where(p => p.Gia <= maxPrice.Value);
            }

            // Logic phân trang
            var totalItems = await productsQuery.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            var products = await productsQuery
                                .Skip((page - 1) * pageSize)
                                .Take(pageSize)
                                .ToListAsync();

            var viewModel = new ShopViewModel
            {
                Products = products,
                Combos = await _context.Combos.AsNoTracking().ToListAsync(), // Tạm thời chưa phân trang combo
                Categories = await _context.LoaiMonAns.AsNoTracking().ToListAsync(),
                SearchTerm = searchTerm,
                CategoryId = categoryId,
                MinPrice = minPrice,
                MaxPrice = maxPrice,
                CurrentPage = page,
                TotalPages = totalPages,
                PageSize = pageSize
            };

            return View(viewModel);
        }

        // Các action khác giữ nguyên...
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var product = await _context.MonAns.AsNoTracking().Include(p => p.LoaiMonAn).FirstOrDefaultAsync(m => m.MaMonAn == id);
            if (product == null) return NotFound();
            var viewModel = new ProductDetailViewModel { Product = product, CartInput = new AddToCartViewModel { ProductId = product.MaMonAn } };
            return View(viewModel);
        }

        public async Task<IActionResult> DetailsCombo(int? id)
        {
            if (id == null) return NotFound();
            var combo = await _context.Combos.AsNoTracking().Include(c => c.ChiTietCombos).ThenInclude(ct => ct.MonAn).FirstOrDefaultAsync(m => m.MaCombo == id);
            if (combo == null) return NotFound();
            return View(combo);
        }
    }
}
