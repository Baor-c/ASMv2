using System.ComponentModel.DataAnnotations;

public class LoaiMonAn
{
    [Key]
    public int MaLoai { get; set; }

    [Required]
    [StringLength(100)]
    public string TenLoai { get; set; }
}
