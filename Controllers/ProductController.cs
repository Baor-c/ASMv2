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
        public async Task<IActionResult> Index(
            string searchTerm, 
            string categoryIds, // Nhận string để parse thành list
            decimal? minPrice, 
            decimal? maxPrice, 
            bool? isPopular,
            bool? isNew,
            string sortBy = "newest",
            int page = 1)
        {
            var pageSize = 12;
            var productsQuery = _context.MonAns
                                        .AsNoTracking() // Tối ưu hiệu năng
                                        .Include(p => p.LoaiMonAn)
                                        .AsQueryable();

            // Tìm kiếm theo tên và mô tả
            if (!string.IsNullOrEmpty(searchTerm))
            {
                productsQuery = productsQuery.Where(p => p.TenMonAn.Contains(searchTerm)
                    || (p.MoTa != null && p.MoTa.Contains(searchTerm)));
            }
            
            // Lọc theo multiple categories
            if (!string.IsNullOrEmpty(categoryIds))
            {
                var categoryIdList = categoryIds.Split(',')
                    .Where(x => int.TryParse(x, out _))
                    .Select(int.Parse)
                    .ToList();
                
                if (categoryIdList.Any())
                {
                    productsQuery = productsQuery.Where(p => categoryIdList.Contains(p.MaLoai));
                }
            }
            
            // Lọc theo khoảng giá
            if (minPrice.HasValue)
            {
                productsQuery = productsQuery.Where(p => p.Gia >= minPrice.Value);
            }
            if (maxPrice.HasValue)
            {
                productsQuery = productsQuery.Where(p => p.Gia <= maxPrice.Value);
            }
            
            // Lọc theo đặc tính sản phẩm
            if (isPopular.HasValue && isPopular.Value)
            {
                productsQuery = productsQuery.Where(p => p.IsPopular);
            }
            if (isNew.HasValue && isNew.Value)
            {
                productsQuery = productsQuery.Where(p => p.IsNew);
            }

            // Sắp xếp
            productsQuery = sortBy switch
            {
                "price_asc" => productsQuery.OrderBy(p => p.Gia),
                "price_desc" => productsQuery.OrderByDescending(p => p.Gia),
                "popular" => productsQuery.OrderByDescending(p => p.IsPopular).ThenByDescending(p => p.Rating),
                "rating" => productsQuery.OrderByDescending(p => p.Rating),
                "newest" or _ => productsQuery.OrderByDescending(p => p.IsNew).ThenByDescending(p => p.MaMonAn)
            };

            // Logic phân trang
            var totalItems = await productsQuery.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            var products = await productsQuery
                                .Skip((page - 1) * pageSize)
                                .Take(pageSize)
                                .ToListAsync();

            // Lấy thông tin về giá để hiển thị price range
            var allPrices = await _context.MonAns.AsNoTracking().Select(p => p.Gia).ToListAsync();
            var minPriceAvailable = allPrices.Any() ? allPrices.Min() : 0;
            var maxPriceAvailable = allPrices.Any() ? allPrices.Max() : 1000000;

            // Parse categoryIds để truyền vào view model
            var selectedCategoryIds = new List<int>();
            if (!string.IsNullOrEmpty(categoryIds))
            {
                selectedCategoryIds = categoryIds.Split(',')
                    .Where(x => int.TryParse(x, out _))
                    .Select(int.Parse)
                    .ToList();
            }

            var viewModel = new ShopViewModel
            {
                Products = products,
                Combos = await _context.Combos.AsNoTracking().ToListAsync(),
                Categories = await _context.LoaiMonAns.AsNoTracking().ToListAsync(),
                SearchTerm = searchTerm,
                CategoryIds = selectedCategoryIds,
                MinPrice = minPrice,
                MaxPrice = maxPrice,
                IsPopular = isPopular,
                IsNew = isNew,
                SortBy = sortBy,
                CurrentPage = page,
                TotalPages = totalPages,
                TotalItems = totalItems,
                PageSize = pageSize,
                MinPriceAvailable = minPriceAvailable,
                MaxPriceAvailable = maxPriceAvailable
            };

            return View(viewModel);
        }

        // Các action khác giữ nguyên...

        public async Task<IActionResult> DetailsCombo(int? id)
        {
            if (id == null) return NotFound();
            var combo = await _context.Combos.AsNoTracking().Include(c => c.ChiTietCombos).ThenInclude(ct => ct.MonAn).FirstOrDefaultAsync(m => m.MaCombo == id);
            if (combo == null) return NotFound();
            return View(combo);
        }

        // API đã bị xóa: Lấy tùy chọn của sản phẩm
    }
}
