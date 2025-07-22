using ASM1.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class DiaChiNguoiDung
{
    [Key]
    public int MaDiaChi { get; set; }

    [Required]
    public int MaNguoiDung { get; set; }
    [ForeignKey("MaNguoiDung")]
    public virtual NguoiDung NguoiDung { get; set; }

    [Required(ErrorMessage = "Tên người nhận không được để trống")]
    [StringLength(100)]
    public string TenNguoiNhan { get; set; }

    [Required(ErrorMessage = "Địa chỉ không được để trống")]
    [StringLength(255)]
    public string DiaChiCuThe { get; set; }

    [Required(ErrorMessage = "Số điện thoại không được để trống")]
    [Phone]
    public string SoDienThoai { get; set; }

    public bool IsDefault { get; set; } = false;
}
