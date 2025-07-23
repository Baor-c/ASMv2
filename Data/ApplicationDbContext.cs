using Microsoft.EntityFrameworkCore;
using ASM1.Models; // Đảm bảo namespace này đúng với project của bạn
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

            // 3. Người dùng (Admin)
            modelBuilder.Entity<NguoiDung>().HasData(
                new NguoiDung { MaNguoiDung = 1, HoTen = "Admin", Email = "admin@example.com", MatKhau = "$2a$11$gA9i54f.gYj02T/Iq0xV..36gT0zQL./C59Fh.Zk.K.dZ.zY.zY.O", SoDienThoai = "0123456789", NgaySinh = new DateTime(1990, 1, 1), MaVaiTro = 1, MaCapThanhVien = 1 }
            );

            // 4. Loại món ăn
            modelBuilder.Entity<LoaiMonAn>().HasData(
                new LoaiMonAn { MaLoai = 1, TenLoai = "Gà Rán & Gà Quay" },
                new LoaiMonAn { MaLoai = 2, TenLoai = "Burger & Cơm" },
                new LoaiMonAn { MaLoai = 3, TenLoai = "Thức Ăn Nhẹ" },
                new LoaiMonAn { MaLoai = 4, TenLoai = "Tráng Miệng & Thức Uống" }
            );

            // 5. Món ăn mẫu - Đã loại bỏ trường HinhAnh khỏi seed data
            // Việc thêm hình ảnh sẽ được thực hiện qua giao diện quản trị của ứng dụng.
            modelBuilder.Entity<MonAn>().HasData(
                new MonAn { MaMonAn = 1, TenMonAn = "1 Miếng Gà Rán Hot & Spicy", MoTa = "Gà Rán Cay Hot & Spicy.", Gia = 36000, MaLoai = 1, IsPopular = true, Rating = 4.8, ReviewCount = 250 },
                new MonAn { MaMonAn = 2, TenMonAn = "3 Miếng Gà Rán Hot & Spicy", MoTa = "Combo 3 miếng Gà Rán Cay Hot & Spicy.", Gia = 105000, MaLoai = 1, IsPopular = true, OriginalPrice = 108000, Rating = 4.7, ReviewCount = 180 },
                new MonAn { MaMonAn = 3, TenMonAn = "1 Miếng Gà Quay Giấy Bạc", MoTa = "Gà Quay Giấy Bạc đậm đà hương vị Việt.", Gia = 69000, MaLoai = 1, IsNew = true, Rating = 4.9, ReviewCount = 150 },
                new MonAn { MaMonAn = 4, TenMonAn = "Burger Zinger", MoTa = "Burger Zinger với 100% thịt phi lê gà cay giòn.", Gia = 55000, MaLoai = 2, IsPopular = true, Rating = 4.9, ReviewCount = 310 },
                new MonAn { MaMonAn = 5, TenMonAn = "Cơm Gà Rán", MoTa = "Cơm Gà Rán kèm xà lách và sốt.", Gia = 45000, MaLoai = 2, Rating = 4.6, ReviewCount = 120 },
                new MonAn { MaMonAn = 6, TenMonAn = "Burger Tôm", MoTa = "Burger nhân tôm tẩm bột chiên giòn, kết hợp với sốt đặc biệt.", Gia = 45000, MaLoai = 2, IsNew = true, Rating = 4.6, ReviewCount = 90 },
                new MonAn { MaMonAn = 7, TenMonAn = "Khoai Tây Chiên (Vừa)", MoTa = "Khoai tây chiên giòn rụm, vàng óng.", Gia = 22000, MaLoai = 3, IsPopular = true, Rating = 4.5, ReviewCount = 450 },
                new MonAn { MaMonAn = 8, TenMonAn = "5 Gà Miếng Nuggets", MoTa = "5 miếng gà viên chiên giòn.", Gia = 42000, MaLoai = 3, Rating = 4.4, ReviewCount = 130 },
                new MonAn { MaMonAn = 9, TenMonAn = "Pepsi Lon", MoTa = "Nước ngọt có gas Pepsi mát lạnh sảng khoái.", Gia = 17000, MaLoai = 4 },
                new MonAn { MaMonAn = 10, TenMonAn = "Bánh Trứng", MoTa = "Bánh trứng nướng thơm lừng, béo ngậy.", Gia = 18000, MaLoai = 4, IsPopular = true, Rating = 4.8, ReviewCount = 280 }
            );

            // 6. Combo mẫu - Đã loại bỏ trường HinhAnh khỏi seed data
            modelBuilder.Entity<Combo>().HasData(
                new Combo { MaCombo = 1, TenCombo = "Combo Zinger", MoTa = "1 Burger Zinger + 1 Khoai Tây Chiên (Vừa) + 1 Pepsi Lon", Gia = 89000 },
                new Combo { MaCombo = 2, TenCombo = "Combo Gà Rán 1 người", MoTa = "2 Miếng Gà Rán + 1 Khoai Tây Chiên (Vừa) + 1 Pepsi Lon", Gia = 92000 },
                new Combo { MaCombo = 3, TenCombo = "Combo Nhóm 2 người", MoTa = "3 Miếng Gà Rán + 1 Burger Tôm + 2 Khoai Tây Chiên (Vừa) + 2 Pepsi Lon", Gia = 199000 },
                new Combo { MaCombo = 4, TenCombo = "Combo Cơm Gà", MoTa = "1 Cơm Gà Rán + 1 Salad + 1 Pepsi Lon", Gia = 69000 }
            );

            // 7. Chi tiết Combo
            modelBuilder.Entity<ChiTietCombo>().HasData(
                new { MaCombo = 1, MaMonAn = 4 }, new { MaCombo = 1, MaMonAn = 7 }, new { MaCombo = 1, MaMonAn = 9 },
                new { MaCombo = 2, MaMonAn = 1 }, new { MaCombo = 2, MaMonAn = 7 }, new { MaCombo = 2, MaMonAn = 9 },
                new { MaCombo = 3, MaMonAn = 1 }, new { MaCombo = 3, MaMonAn = 6 }, new { MaCombo = 3, MaMonAn = 7 }, new { MaCombo = 3, MaMonAn = 9 },
                new { MaCombo = 4, MaMonAn = 5 }, new { MaCombo = 4, MaMonAn = 9 }
            );
        }
    }
}
