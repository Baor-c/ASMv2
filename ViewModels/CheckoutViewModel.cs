using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding; // Thêm using này

namespace FastFoodApp.ViewModels
{
    public class CheckoutViewModel
    {
        [Required(ErrorMessage = "Họ tên không được để trống")]
        [Display(Name = "Họ và Tên")]
        public string HoTen { get; set; }

        [Required(ErrorMessage = "Địa chỉ không được để trống")]
        [Display(Name = "Địa chỉ giao hàng")]
        public string DiaChi { get; set; }

        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        [Display(Name = "Số điện thoại")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        public string SoDienThoai { get; set; }

        [BindNever]
        public List<CartItemViewModel> CartItems { get; set; }
        [Required(ErrorMessage = "Vui lòng chọn một địa chỉ giao hàng.")]
        public int SelectedAddressId { get; set; }

        [BindNever]
        public List<DiaChiNguoiDung> Addresses { get; set; }

        [BindNever]
        public decimal TongTienGoc { get; set; }
        [BindNever]
        public double PhanTramGiamGia { get; set; }
        [BindNever]
        public decimal SoTienGiam { get; set; }
        [BindNever]
        public decimal TongTienThanhToan { get; set; }
    }
}