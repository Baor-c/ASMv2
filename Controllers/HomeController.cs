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
    
    // Phương thức khởi tạo hình ảnh mẫu nếu không có hình ảnh trong database
    private async Task InitializeSampleImagesIfNeeded()
    {
        var monAns = await _context.MonAns.ToListAsync();
        bool hasChanges = false;
        
        // Tạo HttpClient với timeout hợp lý
        using (var httpClient = new HttpClient())
        {
            httpClient.Timeout = TimeSpan.FromSeconds(10);
            
            foreach (var monAn in monAns)
            {
                if (monAn.HinhAnh == null || monAn.HinhAnh.Length == 0)
                {
                    // Mảng URL thay thế để thử lần lượt nếu URL chính không hoạt động
                    var imageUrls = new List<string>();
                    
                    // Thiết lập hình ảnh mẫu dựa vào loại món ăn
                    switch (monAn.MaLoai)
                    {
                        case 1: // Gà
                            imageUrls.Add("https://images.pexels.com/photos/60616/fried-chicken-chicken-fried-crunchy-60616.jpeg");
                            imageUrls.Add("https://images.pexels.com/photos/2338407/pexels-photo-2338407.jpeg");
                            break;
                        case 2: // Burger
                            imageUrls.Add("https://images.pexels.com/photos/1639557/pexels-photo-1639557.jpeg");
                            imageUrls.Add("https://images.pexels.com/photos/1199960/pexels-photo-1199960.jpeg");
                            break;
                        case 3: // Thức ăn nhẹ
                            imageUrls.Add("https://images.pexels.com/photos/1583884/pexels-photo-1583884.jpeg");
                            imageUrls.Add("https://images.pexels.com/photos/1400172/pexels-photo-1400172.jpeg");
                            break;
                        case 4: // Đồ uống
                            imageUrls.Add("https://images.pexels.com/photos/50593/coca-cola-cold-drink-soft-drink-coke-50593.jpeg");
                            imageUrls.Add("https://images.pexels.com/photos/544961/pexels-photo-544961.jpeg");
                            break;
                        default: // Mặc định
                            imageUrls.Add("https://images.pexels.com/photos/1640772/pexels-photo-1640772.jpeg");
                            imageUrls.Add("https://images.pexels.com/photos/958545/pexels-photo-958545.jpeg");
                            break;
                    }

                    bool imageLoaded = false;
                    foreach (var imageUrl in imageUrls)
                    {
                        try
                        {
                            var imageBytes = await httpClient.GetByteArrayAsync(imageUrl);
                            if (imageBytes.Length > 0)
                            {
                                monAn.HinhAnh = imageBytes;
                                hasChanges = true;
                                imageLoaded = true;
                                break;
                            }
                        }
                        catch
                        {
                            // Nếu tải thất bại, thử URL tiếp theo
                            continue;
                        }
                    }
                    
                    // Nếu không tải được hình ảnh nào, sử dụng mặc định từ wwwroot
                    if (!imageLoaded)
                    {
                        // Log không có lỗi thực sự, chỉ để ghi nhận thông tin
                        Console.WriteLine($"Could not load any images for product {monAn.TenMonAn}");
                    }
                }
            }
        }

        if (hasChanges)
        {
            await _context.SaveChangesAsync();
        }
    }
    
    public async Task<IActionResult> Index()
    {
    // Tạo dữ liệu mẫu cho hình ảnh nếu hình ảnh đang null
    await InitializeSampleImagesIfNeeded();

    var viewModel = new HomeViewModel
    {
        LoaiMonAns = await _context.LoaiMonAns.ToListAsync(),

        MonAns = await _context.MonAns
                               .Include(p => p.LoaiMonAn) 
                               .OrderByDescending(p => p.IsPopular)
                               .ThenByDescending(p => p.IsNew)
                               .ThenByDescending(p => p.Rating)
                               .Take(8) 
                               .ToListAsync()
    };    return View(viewModel);
}

    public async Task<IActionResult> Index_test()
    {
        // Tạo dữ liệu mẫu cho hình ảnh nếu hình ảnh đang null
        await InitializeSampleImagesIfNeeded();

        var viewModel = new HomeViewModel
        {
            LoaiMonAns = await _context.LoaiMonAns.ToListAsync(),

            MonAns = await _context.MonAns
                                   .Include(p => p.LoaiMonAn) 
                                   .OrderByDescending(p => p.IsPopular)
                                   .ThenByDescending(p => p.IsNew)
                                   .ThenByDescending(p => p.Rating)
                                   .Take(8) 
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
