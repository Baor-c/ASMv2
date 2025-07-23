using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace FastFoodApp.ViewModels
{
    public class CheckoutViewModel
    {

        [Required(ErrorMessage = "Vui lòng chọn một địa chỉ giao hàng.")]
        public int SelectedAddressId { get; set; }

        [BindNever]
        public List<CartItemViewModel> CartItems { get; set; }

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