using ASM1.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace FastFoodApp.Areas.Admin.Models
{
    public class AdminProductsViewModel
    {
        public List<MonAn> Products { get; set; }
        public SelectList Categories { get; set; }

        public string SearchTerm { get; set; }
        public int? CategoryId { get; set; }
    }
}
