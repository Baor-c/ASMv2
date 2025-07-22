using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ASM1.Models;

namespace FastFoodApp.ViewModels
{
    public class ProfileViewModel
    {
        public int MaNguoiDung { get; set; }
        public string Email { get; set; }

        [Required(ErrorMessage = "Họ tên không được để trống")]
        [Display(Name = "Họ và Tên")]
        public string HoTen { get; set; }

        [Display(Name = "Số điện thoại")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        public string SoDienThoai { get; set; }

        [Display(Name = "Ngày sinh")]
        [DataType(DataType.Date)]
        public DateTime? NgaySinh { get; set; }

        public int TotalOrders { get; set; }
        public decimal TotalSpent { get; set; }
        public int CompletedOrders { get; set; }
        public CapThanhVien CurrentMembership { get; set; }
        public List<CapThanhVien> AllMemberships { get; set; }

        public CapThanhVien NextMembership { get; set; }
        public double ProgressToNextLevel { get; set; }

        public List<DiaChiNguoiDung> Addresses { get; set; }
        public DiaChiNguoiDung NewAddress { get; set; }

        public ProfileViewModel()
        {
            Addresses = new List<DiaChiNguoiDung>();
            NewAddress = new DiaChiNguoiDung();
        }
    }
}
