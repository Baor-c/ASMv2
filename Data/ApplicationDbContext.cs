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
        public DbSet<CartItem> CartItems { get; set; }

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
                new CapThanhVien { MaCapThanhVien = 2, TenCap = "Bạc", NguongDiem = 300, PhanTramGiamGia = 5 },
                new CapThanhVien { MaCapThanhVien = 3, TenCap = "Vàng", NguongDiem = 500, PhanTramGiamGia = 10 }
            );

            modelBuilder.Entity<NguoiDung>().HasData(
                new NguoiDung { MaNguoiDung = 1, HoTen = "Admin", Email = "admin@example.com", MatKhau = "$2a$11$gA9i54f.gYj02T/Iq0xV..36gT0zQL./C59Fh.Zk.K.dZ.zY.zY.O", SoDienThoai = "0123456789", NgaySinh = new DateTime(1990, 1, 1), MaVaiTro = 1, MaCapThanhVien = 1 }
            );

            modelBuilder.Entity<LoaiMonAn>().HasData(
                new LoaiMonAn { MaLoai = 1, TenLoai = "Gà Rán & Gà Quay" },
                new LoaiMonAn { MaLoai = 2, TenLoai = "Burger & Cơm" },
                new LoaiMonAn { MaLoai = 3, TenLoai = "Thức Ăn Nhẹ" },
                new LoaiMonAn { MaLoai = 4, TenLoai = "Tráng Miệng & Thức Uống" }
            );

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
                new MonAn { MaMonAn = 10, TenMonAn = "Bánh Trứng", MoTa = "Bánh trứng nướng thơm lừng, béo ngậy.", Gia = 18000, MaLoai = 4, IsPopular = true, Rating = 4.8, ReviewCount = 280 },
                
                // 20 sản phẩm thức ăn bổ sung
                new MonAn { MaMonAn = 11, TenMonAn = "1 Miếng Gà Rán Original", MoTa = "Gà rán không cay, phù hợp cho người không ăn được cay.", Gia = 39000, MaLoai = 1, Rating = 4.6, ReviewCount = 180 },
                new MonAn { MaMonAn = 12, TenMonAn = "3 Miếng Gà Rán Original", MoTa = "Combo 3 miếng Gà Rán Original không cay.", Gia = 115000, MaLoai = 1, IsPopular = true, OriginalPrice = 120000, Rating = 4.6, ReviewCount = 170 },
                new MonAn { MaMonAn = 13, TenMonAn = "1 Miếng Gà Quay Mật Ong", MoTa = "Gà Quay Mật Ong thơm ngon, da giòn.", Gia = 75000, MaLoai = 1, IsNew = true, Rating = 4.8, ReviewCount = 220 },
                new MonAn { MaMonAn = 14, TenMonAn = "Đùi Gà Nướng BBQ", MoTa = "Đùi gà nướng với sốt BBQ đặc trưng.", Gia = 78000, MaLoai = 1, IsNew = true, Rating = 4.7, ReviewCount = 160 },
                new MonAn { MaMonAn = 15, TenMonAn = "Burger Phô Mai Gà", MoTa = "Burger gà với sốt phô mai béo ngậy.", Gia = 59000, MaLoai = 2, Rating = 4.7, ReviewCount = 280 },
                new MonAn { MaMonAn = 16, TenMonAn = "Burger Bò Phô Mai Đôi", MoTa = "Burger bò 2 lớp kẹp phô mai.", Gia = 69000, MaLoai = 2, Rating = 4.8, ReviewCount = 320 },
                new MonAn { MaMonAn = 17, TenMonAn = "Cơm Gà Giòn Cay", MoTa = "Cơm với gà phi-lê tẩm bột chiên giòn.", Gia = 59000, MaLoai = 2, IsPopular = true, Rating = 4.6, ReviewCount = 190 },
                new MonAn { MaMonAn = 18, TenMonAn = "Cơm Bò Tiêu Đen", MoTa = "Cơm với thịt bò xào cùng sốt tiêu đen.", Gia = 65000, MaLoai = 2, IsNew = true, Rating = 4.7, ReviewCount = 145 },
                new MonAn { MaMonAn = 19, TenMonAn = "Mì Ý Sốt Bò Bằm", MoTa = "Mì Ý sốt bò bằm đậm đà.", Gia = 68000, MaLoai = 2, Rating = 4.5, ReviewCount = 130 },
                new MonAn { MaMonAn = 20, TenMonAn = "3 Miếng Cánh Gà Giòn", MoTa = "Cánh gà tẩm bột chiên giòn.", Gia = 49000, MaLoai = 3, IsPopular = true, Rating = 4.7, ReviewCount = 290 },
                new MonAn { MaMonAn = 21, TenMonAn = "Khoai Tây Phô Mai", MoTa = "Khoai tây chiên với phô mai tan chảy bên trên.", Gia = 29000, MaLoai = 3, Rating = 4.6, ReviewCount = 220 },
                new MonAn { MaMonAn = 22, TenMonAn = "2 Bánh Mì Que Tỏi", MoTa = "Bánh mì que giòn tan phủ tỏi thơm.", Gia = 39000, MaLoai = 3, Rating = 4.3, ReviewCount = 110 },
                new MonAn { MaMonAn = 23, TenMonAn = "Salad Caesar", MoTa = "Xà lách trộn với sốt Caesar và bánh mì nướng.", Gia = 45000, MaLoai = 3, Rating = 4.4, ReviewCount = 95 },
                new MonAn { MaMonAn = 24, TenMonAn = "Súp Kem Nấm", MoTa = "Súp kem nấm thơm béo.", Gia = 35000, MaLoai = 3, Rating = 4.5, ReviewCount = 120 },
                new MonAn { MaMonAn = 25, TenMonAn = "Coca Cola Lon", MoTa = "Coca Cola lon mát lạnh.", Gia = 22000, MaLoai = 4, IsPopular = true, Rating = 4.5, ReviewCount = 380 },
                new MonAn { MaMonAn = 26, TenMonAn = "Sprite Lon", MoTa = "Sprite lon mát lạnh.", Gia = 24000, MaLoai = 4, Rating = 4.4, ReviewCount = 210 },
                new MonAn { MaMonAn = 27, TenMonAn = "Trà Đào", MoTa = "Trà đào thơm mát với đào miếng.", Gia = 29000, MaLoai = 4, IsNew = true, Rating = 4.8, ReviewCount = 250 },
                new MonAn { MaMonAn = 28, TenMonAn = "Trà Chanh", MoTa = "Trà chanh thơm mát.", Gia = 27000, MaLoai = 4, Rating = 4.6, ReviewCount = 180 },
                new MonAn { MaMonAn = 29, TenMonAn = "Kem Sundae Socola", MoTa = "Kem sundae phủ socola đậm đà.", Gia = 35000, MaLoai = 4, IsPopular = true, IsNew = true, Rating = 4.9, ReviewCount = 300 },
                new MonAn { MaMonAn = 30, TenMonAn = "Kem Sundae Dâu", MoTa = "Kem sundae phủ dâu tây ngọt ngào.", Gia = 32000, MaLoai = 4, Rating = 4.7, ReviewCount = 240 }
            );

            modelBuilder.Entity<Combo>().HasData(
                new Combo { MaCombo = 1, TenCombo = "Combo Zinger", MoTa = "1 Burger Zinger + 1 Khoai Tây Chiên (Vừa) + 1 Pepsi Lon", Gia = 89000 },
                new Combo { MaCombo = 2, TenCombo = "Combo Gà Rán 1 người", MoTa = "2 Miếng Gà Rán + 1 Khoai Tây Chiên (Vừa) + 1 Pepsi Lon", Gia = 92000 },
                new Combo { MaCombo = 3, TenCombo = "Combo Nhóm 2 người", MoTa = "3 Miếng Gà Rán + 1 Burger Tôm + 2 Khoai Tây Chiên (Vừa) + 2 Pepsi Lon", Gia = 199000 },
                new Combo { MaCombo = 4, TenCombo = "Combo Cơm Gà", MoTa = "1 Cơm Gà Rán + 1 Salad + 1 Pepsi Lon", Gia = 69000 }
            );

            modelBuilder.Entity<ChiTietCombo>().HasData(
                new { MaCombo = 1, MaMonAn = 4 },
                new { MaCombo = 1, MaMonAn = 7 },
                new { MaCombo = 1, MaMonAn = 9 },
                new { MaCombo = 2, MaMonAn = 1 }, 
                new { MaCombo = 2, MaMonAn = 7 },
                new { MaCombo = 2, MaMonAn = 9 },
                new { MaCombo = 3, MaMonAn = 1 },
                new { MaCombo = 3, MaMonAn = 6 }, 
                new { MaCombo = 3, MaMonAn = 7 }, 
                new { MaCombo = 3, MaMonAn = 9 },
                new { MaCombo = 4, MaMonAn = 5 },
                new { MaCombo = 4, MaMonAn = 9 }
            );
        }
    }
}
