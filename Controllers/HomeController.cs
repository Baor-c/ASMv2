using FastFoodApp.Data;
using FastFoodApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace FastFoodApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = new HomeViewModel
            {
                LoaiMonAns = await _context.LoaiMonAns.ToListAsync(),

                // Cập nhật logic: Lấy các món ăn phổ biến và mới nhất
                MonAns = await _context.MonAns
                                       .Include(p => p.LoaiMonAn) // Include LoaiMonAn để hiển thị tên loại
                                       .OrderByDescending(p => p.IsPopular)
                                       .ThenByDescending(p => p.IsNew)
                                       .ThenByDescending(p => p.Rating)
                                       .Take(6) // Lấy 6 món nổi bật nhất
                                       .ToListAsync()
            };

            return View(viewModel);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
    }
}
