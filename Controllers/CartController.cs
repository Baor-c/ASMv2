using ASM1.Models;
using FastFoodApp.Data;
using FastFoodApp.Helpers; 
using FastFoodApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FastFoodApp.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;
        public const string CARTKEY = "cart";

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        List<CartItemViewModel> Carts => HttpContext.Session.Get<List<CartItemViewModel>>(CARTKEY) ?? new List<CartItemViewModel>();

        public async Task<IActionResult> Index()
        {
            var cartItems = Carts;

            var recommended = await _context.MonAns
                                            .Where(m => m.MaLoai == 4)
                                            .Take(4)
                                            .AsNoTracking()
                                            .ToListAsync();

            var viewModel = new CartViewModel
            {
                CartItems = cartItems,
                RecommendedProducts = recommended
            };

            return View(viewModel);
        }

        private IActionResult AddItemToCart(int id, bool isCombo, int quantity)
        {
            var gioHang = Carts;
            var item = gioHang.SingleOrDefault(p => p.ItemId == id && p.IsCombo == isCombo);

            if (item == null)
            {
                if (isCombo)
                {
                    var combo = _context.Combos.SingleOrDefault(p => p.MaCombo == id);
                    if (combo == null) return NotFound();
                    item = new CartItemViewModel
                    {
                        ItemId = combo.MaCombo,
                        TenSanPham = combo.TenCombo,
                        DonGia = combo.Gia,
                        SoLuong = quantity,
                        HinhAnh = Convert.ToBase64String(combo.HinhAnh ?? new byte[0]),
                        IsCombo = true
                    };
                }
                else
                {
                    var monAn = _context.MonAns.SingleOrDefault(p => p.MaMonAn == id);
                    if (monAn == null) return NotFound();
                    item = new CartItemViewModel
                    {
                        ItemId = monAn.MaMonAn,
                        TenSanPham = monAn.TenMonAn,
                        DonGia = monAn.Gia,
                        SoLuong = quantity,
                        HinhAnh = Convert.ToBase64String(monAn.HinhAnh ?? new byte[0]),
                        IsCombo = false
                    };
                }
                gioHang.Add(item);
            }
            else
            {
                item.SoLuong += quantity;
            }

            HttpContext.Session.Set(CARTKEY, gioHang);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult AddToCart(int id, int quantity = 1)
        {
            return AddItemToCart(id, false, quantity);
        }

        [HttpPost]
        public IActionResult AddComboToCart(int id, int quantity = 1)
        {
            return AddItemToCart(id, true, quantity);
        }

        [HttpPost]
        public IActionResult RemoveCartItem(int itemId, bool isCombo)
        {
            var gioHang = Carts;
            var item = gioHang.SingleOrDefault(p => p.ItemId == itemId && p.IsCombo == isCombo);

            if (item != null)
            {
                gioHang.Remove(item);
                HttpContext.Session.Set(CARTKEY, gioHang);
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddToCartJson(int id, int quantity = 1, bool isCombo = false)
        {
            Console.WriteLine($"AddToCartJson called: id={id}, quantity={quantity}, isCombo={isCombo}");
            var gioHang = Carts;
            var item = gioHang.SingleOrDefault(p => p.ItemId == id && p.IsCombo == isCombo);

            if (item == null)
            {
                if (isCombo)
                {
                    var combo = _context.Combos.Find(id);
                    if (combo == null) return Json(new { success = false, message = "Sản phẩm không tồn tại." });
                    item = new CartItemViewModel { 
                        ItemId = combo.MaCombo, 
                        TenSanPham = combo.TenCombo, 
                        DonGia = combo.Gia, 
                        SoLuong = quantity, 
                        HinhAnh = combo.HinhAnh != null ? Convert.ToBase64String(combo.HinhAnh) : string.Empty,
                        IsCombo = true 
                    };
                }
                else
                {
                    var monAn = _context.MonAns.Find(id);
                    if (monAn == null) return Json(new { success = false, message = "Sản phẩm không tồn tại." });
                    item = new CartItemViewModel { 
                        ItemId = monAn.MaMonAn, 
                        TenSanPham = monAn.TenMonAn, 
                        DonGia = monAn.Gia, 
                        SoLuong = quantity, 
                        HinhAnh = monAn.HinhAnh != null ? Convert.ToBase64String(monAn.HinhAnh) : string.Empty,
                        IsCombo = false 
                    };
                }
                gioHang.Add(item);
            }
            else
            {
                item.SoLuong += quantity;
            }

            HttpContext.Session.Set(CARTKEY, gioHang);
            return Json(new { success = true, message = "Thêm vào giỏ hàng thành công!", cartCount = gioHang.Sum(i => i.SoLuong) });
        }

        [HttpGet]
        public IActionResult GetCartCount()
        {
            return Json(new { count = Carts.Sum(i => i.SoLuong) });
        }
        
        [HttpGet]
        [Route("Cart/QuickAdd")]
        public IActionResult QuickAddToCart(int id, int quantity = 1, bool isCombo = false, bool redirectToCart = false, decimal extraCost = 0)
        {
            var gioHang = Carts;
            
            // Find the item in cart by ID and type
            var item = gioHang.SingleOrDefault(p => p.ItemId == id && p.IsCombo == isCombo);

            if (item == null)
            {
                if (isCombo)
                {
                    var combo = _context.Combos.Find(id);
                    if (combo == null) return Json(new { success = false, message = "Sản phẩm không tồn tại." });
                    
                    // Create cart item for combo
                    string productName = combo.TenCombo;
                    
                    item = new CartItemViewModel { 
                        ItemId = combo.MaCombo, 
                        TenSanPham = productName, 
                        DonGia = combo.Gia + extraCost, 
                        SoLuong = quantity, 
                        HinhAnh = combo.HinhAnh != null ? Convert.ToBase64String(combo.HinhAnh) : string.Empty,
                        IsCombo = true,
                        TenMonAn = string.Empty
                    };
                }
                else
                {
                    var monAn = _context.MonAns.Find(id);
                    if (monAn == null) return Json(new { success = false, message = "Sản phẩm không tồn tại." });
                    
                    // Create cart item for food
                    string productName = monAn.TenMonAn;
                    
                    item = new CartItemViewModel { 
                        ItemId = monAn.MaMonAn, 
                        TenSanPham = productName, 
                        DonGia = monAn.Gia + extraCost, 
                        SoLuong = quantity, 
                        HinhAnh = monAn.HinhAnh != null ? Convert.ToBase64String(monAn.HinhAnh) : string.Empty,
                        IsCombo = false,
                        TenMonAn = string.Empty,
                        MaMonAn = monAn.MaMonAn
                    };
                }
                gioHang.Add(item);
            }
            else
            {
                item.SoLuong += quantity;
            }

            HttpContext.Session.Set(CARTKEY, gioHang);
            
            if (redirectToCart)
            {
                return RedirectToAction("Index");
            }
            
            return Json(new { success = true, message = "Thêm vào giỏ hàng thành công!", cartCount = gioHang.Sum(i => i.SoLuong) });
        }
        [HttpPost]
        public IActionResult UpdateCartItem(int itemId, bool isCombo, int quantity)
        {
            var gioHang = Carts;
            var item = gioHang.SingleOrDefault(p => p.ItemId == itemId && p.IsCombo == isCombo);

            if (item != null)
            {
                if (quantity > 0)
                {
                    item.SoLuong = quantity;
                }
                else
                {
                    gioHang.Remove(item);
                }
                HttpContext.Session.Set(CARTKEY, gioHang);
            }

            return RedirectToAction("Index");
        }
        [HttpPost]
        // AddToCartWithOptions method has been removed as part of option-related functionality cleanup

        private string GetCartSessionId()
        {
            string? cartSessionId = HttpContext.Session.GetString("CartSessionId");
            if (cartSessionId == null)
            {
                // Nếu chưa có session ID, tạo một cái mới
                Guid tempCartId = Guid.NewGuid();
                cartSessionId = tempCartId.ToString();
                HttpContext.Session.SetString("CartSessionId", cartSessionId);
            }
            return cartSessionId;
        }
    }
}
