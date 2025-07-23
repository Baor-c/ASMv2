using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ASM1.Models
{
    public class Combo
    {
        [Key]
        public int MaCombo { get; set; }

        [Required]
        [StringLength(150)]
        public string TenCombo { get; set; }

        public string MoTa { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Gia { get; set; }

        public byte[]? HinhAnh { get; set; }

        public virtual ICollection<ChiTietCombo> ChiTietCombos { get; set; }
    }
}