using ASM1.Models;
using System.ComponentModel.DataAnnotations.Schema;

public class ChiTietCombo
{
    public int MaCombo { get; set; }
    [ForeignKey("MaCombo")]
    public virtual Combo Combo { get; set; }

    public int MaMonAn { get; set; }
    [ForeignKey("MaMonAn")]
    public virtual MonAn MonAn { get; set; }
}