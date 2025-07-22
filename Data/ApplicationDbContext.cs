using Microsoft.EntityFrameworkCore;
using ASM1.Models;
using System;
namespace FastFoodApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<HoaDon> HoaDons { get; set; }
        public DbSet<ChiTietHoaDon> ChiTietHoaDons { get; set; }

        public DbSet<DiaChiNguoiDung> DiaChiNguoiDungs { get; set; }
        public DbSet<NguoiDung> NguoiDungs { get; set; }
        public DbSet<VaiTro> VaiTros { get; set; }
        public DbSet<CapThanhVien> CapThanhViens { get; set; }
        public DbSet<LoaiMonAn> LoaiMonAns { get; set; }
        public DbSet<MonAn> MonAns { get; set; }
        public DbSet<Combo> Combos { get; set; }
        public DbSet<ChiTietCombo> ChiTietCombos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ChiTietCombo>()
                .HasKey(ctc => new { ctc.MaCombo, ctc.MaMonAn });

            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // 1. Vai trò
            modelBuilder.Entity<VaiTro>().HasData(
                new VaiTro { MaVaiTro = 1, TenVaiTro = "Admin" },
                new VaiTro { MaVaiTro = 2, TenVaiTro = "Customer" }
            );

            // 2. Cấp thành viên
            modelBuilder.Entity<CapThanhVien>().HasData(
                new CapThanhVien { MaCapThanhVien = 1, TenCap = "Đồng", NguongDiem = 0, PhanTramGiamGia = 0 },
                new CapThanhVien { MaCapThanhVien = 2, TenCap = "Bạc", NguongDiem = 1000, PhanTramGiamGia = 5 },
                new CapThanhVien { MaCapThanhVien = 3, TenCap = "Vàng", NguongDiem = 5000, PhanTramGiamGia = 10 }
            );

            // 3. Người dùng Admin mẫu
            modelBuilder.Entity<NguoiDung>().HasData(
                new NguoiDung
                {
                    MaNguoiDung = 1,
                    HoTen = "Admin",
                    Email = "admin@example.com",
                    MatKhau = "$2a$11$gA9i54f.gYj02T/Iq0xV..36gT0zQL./C59Fh.Zk.K.dZ.zY.zY.O",
                    SoDienThoai = "0123456789",
                    DiaChi = "123 Admin Street",
                    NgaySinh = new DateTime(1990, 1, 1),
                    MaVaiTro = 1,
                    MaCapThanhVien = 1
                }
            );

            // 4. Loại món ăn
            modelBuilder.Entity<LoaiMonAn>().HasData(
                new LoaiMonAn { MaLoai = 1, TenLoai = "Gà Rán" },
                new LoaiMonAn { MaLoai = 2, TenLoai = "Burger" },
                new LoaiMonAn { MaLoai = 3, TenLoai = "Nước Uống" },
                new LoaiMonAn { MaLoai = 4, TenLoai = "Món Ăn Kèm" }
            );

            // 5. Món ăn mẫu (ĐÃ SỬA LỖI THAM CHIẾU)
            modelBuilder.Entity<MonAn>().HasData(
                new MonAn { MaMonAn = 1, TenMonAn = "1 Miếng Gà Rán Truyền Thống", MoTa = "Gà rán giòn tan với công thức 11 loại thảo mộc và gia vị.", Gia = 35000, MaLoai = 1, IsPopular = true, IsNew = true, OriginalPrice = 40000, Rating = 4.8, ReviewCount = 152 },
                new MonAn { MaMonAn = 2, TenMonAn = "1 Miếng Gà Rán Cay", MoTa = "Gà rán cay nồng, đậm vị cho người thích ăn cay.", Gia = 36000, MaLoai = 1, IsPopular = true, IsNew = false, Rating = 4.7, ReviewCount = 130 },
                new MonAn { MaMonAn = 3, TenMonAn = "Burger Zinger", MoTa = "Burger với miếng phi lê gà cay giòn, rau diếp tươi và sốt mayonnaise.", Gia = 55000, MaLoai = 2, IsPopular = true, IsNew = false, OriginalPrice = 65000, Rating = 4.9, ReviewCount = 210 },
                new MonAn { MaMonAn = 4, TenMonAn = "Burger Tôm", MoTa = "Burger nhân tôm tẩm bột chiên giòn, kết hợp với sốt đặc biệt.", Gia = 45000, MaLoai = 2, IsNew = true, Rating = 4.6, ReviewCount = 88 },
                new MonAn { MaMonAn = 5, TenMonAn = "Khoai Tây Chiên (Vừa)", MoTa = "Khoai tây chiên giòn rụm, vàng óng.", Gia = 20000, MaLoai = 4, IsPopular = true, Rating = 4.5, ReviewCount = 350 },
                new MonAn { MaMonAn = 6, TenMonAn = "Pepsi Lon", MoTa = "Nước ngọt có gas Pepsi mát lạnh.", Gia = 15000, MaLoai = 3 },
                new MonAn { MaMonAn = 7, TenMonAn = "7 Up Lon", MoTa = "Nước ngọt có gas 7 Up vị chanh.", Gia = 15000, MaLoai = 3 }
            );

            // 6. Combo mẫu
            modelBuilder.Entity<Combo>().HasData(
                new Combo { MaCombo = 1, TenCombo = "Combo Gà Rán A", MoTa = "Bao gồm 1 Gà Rán Truyền Thống, 1 Khoai Tây Chiên (Vừa), 1 Pepsi Lon.", Gia = 65000 },
                new Combo { MaCombo = 2, TenCombo = "Combo Burger Zinger", MoTa = "Bao gồm 1 Burger Zinger, 1 Khoai Tây Chiên (Vừa), 1 Pepsi Lon.", Gia = 85000 },
                new Combo { MaCombo = 3, TenCombo = "Combo Nhóm Vui Vẻ", MoTa = "Bao gồm 3 Gà Rán Truyền Thống, 2 Khoai Tây Chiên (Vừa), 2 Pepsi Lon.", Gia = 159000 }
            );

            // 7. Chi tiết Combo
            modelBuilder.Entity<ChiTietCombo>().HasData(
                // Combo 1
                new { MaCombo = 1, MaMonAn = 1 },
                new { MaCombo = 1, MaMonAn = 5 },
                new { MaCombo = 1, MaMonAn = 6 },

                // Combo 2
                new { MaCombo = 2, MaMonAn = 3 },
                new { MaCombo = 2, MaMonAn = 5 },
                new { MaCombo = 2, MaMonAn = 6 },

                // Combo 3
                new { MaCombo = 3, MaMonAn = 2 },
                new { MaCombo = 3, MaMonAn = 5 },
                new { MaCombo = 3, MaMonAn = 7 }
            );
        }
    }
}
