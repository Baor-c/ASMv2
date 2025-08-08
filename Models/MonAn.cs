using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASM1.Models
{
    public class MonAn
    {
        [Key]
        public int MaMonAn { get; set; }

        [Required]
        [StringLength(150)]
        public string TenMonAn { get; set; } = null!;

        public string? MoTa { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Gia { get; set; }

        public decimal? OriginalPrice { get; set; } 
        public bool IsPopular { get; set; } = false;
        public bool IsNew { get; set; } = false;
        public double Rating { get; set; } = 4.5; 
        public int ReviewCount { get; set; } = 100; 

        public byte[]? HinhAnh { get; set; }

        public int MaLoai { get; set; }
        [ForeignKey("MaLoai")]
        public virtual LoaiMonAn LoaiMonAn { get; set; } = null!;
    }
}
