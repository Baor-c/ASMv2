using FastFoodApp.Helpers;
using FastFoodApp.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FastFoodApp.ViewComponents
{
    public class CartViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var cart = HttpContext.Session.Get<List<CartItemViewModel>>("cart") ?? new List<CartItemViewModel>();
            return View(cart);
        }
    }
}