using System.ComponentModel.DataAnnotations;

public class CapThanhVien
{
    [Key]
    public int MaCapThanhVien { get; set; }

    [Required]
    [StringLength(50)]
    public string TenCap { get; set; }

    public int NguongDiem { get; set; }

    public double PhanTramGiamGia { get; set; }
}

