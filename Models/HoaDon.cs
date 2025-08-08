using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASM1.Models
{
    public class HoaDon
    {
        [Key] // Dòng này rất quan trọng để xác định khóa chính
        public int MaHoaDon { get; set; }

        public DateTime NgayDat { get; set; }

        public decimal TongTien { get; set; }

        [StringLength(50)]
        public string TrangThai { get; set; }

        [StringLength(255)]
        public string DiaChiGiaoHang { get; set; }

        [StringLength(50)]
        public string? PhuongThucThanhToan { get; set; }

        // Khóa ngoại
        public int MaNguoiDung { get; set; }
        [ForeignKey("MaNguoiDung")]
        public virtual NguoiDung NguoiDung { get; set; }

        // Navigation property
        public virtual ICollection<ChiTietHoaDon> ChiTietHoaDons { get; set; }
    }
}