using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace FastFoodApp.ViewModels
{
    public class CheckoutViewModel
    {

        [Required(ErrorMessage = "Vui lòng chọn một địa chỉ giao hàng.")]
        public int SelectedAddressId { get; set; }
        
        [Required(ErrorMessage = "Vui lòng chọn phương thức thanh toán.")]
        public string PaymentMethod { get; set; } = "COD";

        [BindNever]
        public List<CartItemViewModel> CartItems { get; set; } = new();

        [BindNever]
        public List<DiaChiNguoiDung> Addresses { get; set; } = new();

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