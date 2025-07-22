using ASM1.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class ChiTietHoaDon
{
    [Key]
    public int MaChiTiet { get; set; }

    public int SoLuong { get; set; }

    public decimal DonGia { get; set; }

    public int MaHoaDon { get; set; }
    [ForeignKey("MaHoaDon")]
    public virtual HoaDon HoaDon { get; set; }

    public int? MaMonAn { get; set; }
    [ForeignKey("MaMonAn")]
    public virtual MonAn MonAn { get; set; }

    public int? MaCombo { get; set; }
    [ForeignKey("MaCombo")]
    public virtual Combo Combo { get; set; }
}
