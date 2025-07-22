using ASM1.Models;
using System.Collections.Generic;

namespace FastFoodApp.ViewModels
{
    public class ShopViewModel
    {
        public List<MonAn> Products { get; set; }
        public List<LoaiMonAn> Categories { get; set; }
        public List<Combo> Combos { get; set; }

        public string SearchTerm { get; set; }
        public int? CategoryId { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }

        public bool IsPopular { get; set; }
        public bool IsNew { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; } = 8;

    }
}
