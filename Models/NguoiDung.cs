using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASM1.Models
{
    public class NguoiDung
    {
        [Key]
        public int MaNguoiDung { get; set; }

        [Required(ErrorMessage = "Họ tên không được để trống")]
        [StringLength(100)]
        public string HoTen { get; set; }

        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress]
        public string Email { get; set; }

        [StringLength(255)]
        public string? MatKhau { get; set; }

        [Phone]
        public string? SoDienThoai { get; set; }

        public DateTime? NgaySinh { get; set; }
        public int DiemTichLuy { get; set; } = 0;

        public int MaVaiTro { get; set; }
        [ForeignKey("MaVaiTro")]
        public virtual VaiTro VaiTro { get; set; }

        public int MaCapThanhVien { get; set; }
        [ForeignKey("MaCapThanhVien")]
        public virtual CapThanhVien CapThanhVien { get; set; }

        public virtual ICollection<DiaChiNguoiDung> DiaChis { get; set; }
    }
}
