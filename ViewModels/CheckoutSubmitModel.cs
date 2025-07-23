
using System.ComponentModel.DataAnnotations;

namespace FastFoodApp.ViewModels
{
    public class CheckoutSubmitModel
    {
        [Required(ErrorMessage = "Vui lòng chọn một địa chỉ giao hàng.")]
        [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn một địa chỉ giao hàng hợp lệ.")]
        public int SelectedAddressId { get; set; }

    }
}