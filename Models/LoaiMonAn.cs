using System.ComponentModel.DataAnnotations;
using ASM1.Models;
namespace ASM1.Models;
public class LoaiMonAn
{
    [Key]
    public int MaLoai { get; set; }

    [Required]
    [StringLength(100)]
    public string TenLoai { get; set; }
}
