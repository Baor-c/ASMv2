using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class DiaChiNguoiDung
{
    [Key]
    public int MaDiaChi { get; set; }

    [Required(ErrorMessage = "Họ tên người nhận không được để trống")]
    [StringLength(100)]
    public string TenNguoiNhan { get; set; }

    [Required(ErrorMessage = "Số điện thoại không được để trống")]
    [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
    public string SoDienThoai { get; set; }

    [Required(ErrorMessage = "Địa chỉ cụ thể không được để trống")]
    [StringLength(255)]
    public string DiaChiCuThe { get; set; }

    public bool IsDefault { get; set; }

    public int MaNguoiDung { get; set; }
    [ForeignKey("MaNguoiDung")]

    [ValidateNever]
    public virtual ASM1.Models.NguoiDung NguoiDung { get; set; }
}