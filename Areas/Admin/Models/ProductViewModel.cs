using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using ASM1.Models;

namespace FastFoodApp.Areas.Admin.Models
{
    public class ProductViewModel
    {
        public int MaMonAn { get; set; }

        [Required(ErrorMessage = "Tên món ăn là bắt buộc.")]
        [StringLength(100, ErrorMessage = "Tên món ăn không được vượt quá 100 ký tự.")]
        public string TenMonAn { get; set; }

        [Required(ErrorMessage = "Mô tả là bắt buộc.")]
        public string MoTa { get; set; }

        [Required(ErrorMessage = "Giá là bắt buộc.")]
        [Range(0.01, 10000000, ErrorMessage = "Giá phải là một số dương.")]
        public decimal Gia { get; set; }

        public byte[]? HinhAnh { get; set; } // Dữ liệu ảnh hiện có

        public IFormFile? HinhAnhFile { get; set; } // File ảnh mới tải lên

        [Required(ErrorMessage = "Vui lòng chọn loại món ăn.")]
        public int MaLoai { get; set; }

        public LoaiMonAn? LoaiMonAn { get; set; }
    }
}
