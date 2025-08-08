using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FastFoodApp.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CapThanhViens",
                columns: table => new
                {
                    MaCapThanhVien = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenCap = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NguongDiem = table.Column<int>(type: "int", nullable: false),
                    PhanTramGiamGia = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CapThanhViens", x => x.MaCapThanhVien);
                });

            migrationBuilder.CreateTable(
                name: "Combos",
                columns: table => new
                {
                    MaCombo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenCombo = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Gia = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    HinhAnh = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Combos", x => x.MaCombo);
                });

            migrationBuilder.CreateTable(
                name: "LoaiMonAns",
                columns: table => new
                {
                    MaLoai = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenLoai = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoaiMonAns", x => x.MaLoai);
                });

            migrationBuilder.CreateTable(
                name: "VaiTros",
                columns: table => new
                {
                    MaVaiTro = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenVaiTro = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VaiTros", x => x.MaVaiTro);
                });

            migrationBuilder.CreateTable(
                name: "MonAns",
                columns: table => new
                {
                    MaMonAn = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenMonAn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Gia = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OriginalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IsPopular = table.Column<bool>(type: "bit", nullable: false),
                    IsNew = table.Column<bool>(type: "bit", nullable: false),
                    Rating = table.Column<double>(type: "float", nullable: false),
                    ReviewCount = table.Column<int>(type: "int", nullable: false),
                    HinhAnh = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    MaLoai = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonAns", x => x.MaMonAn);
                    table.ForeignKey(
                        name: "FK_MonAns_LoaiMonAns_MaLoai",
                        column: x => x.MaLoai,
                        principalTable: "LoaiMonAns",
                        principalColumn: "MaLoai",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NguoiDungs",
                columns: table => new
                {
                    MaNguoiDung = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HoTen = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MatKhau = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    SoDienThoai = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NgaySinh = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DiemTichLuy = table.Column<int>(type: "int", nullable: false),
                    MaVaiTro = table.Column<int>(type: "int", nullable: false),
                    MaCapThanhVien = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NguoiDungs", x => x.MaNguoiDung);
                    table.ForeignKey(
                        name: "FK_NguoiDungs_CapThanhViens_MaCapThanhVien",
                        column: x => x.MaCapThanhVien,
                        principalTable: "CapThanhViens",
                        principalColumn: "MaCapThanhVien",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NguoiDungs_VaiTros_MaVaiTro",
                        column: x => x.MaVaiTro,
                        principalTable: "VaiTros",
                        principalColumn: "MaVaiTro",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChiTietCombos",
                columns: table => new
                {
                    MaCombo = table.Column<int>(type: "int", nullable: false),
                    MaMonAn = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietCombos", x => new { x.MaCombo, x.MaMonAn });
                    table.ForeignKey(
                        name: "FK_ChiTietCombos_Combos_MaCombo",
                        column: x => x.MaCombo,
                        principalTable: "Combos",
                        principalColumn: "MaCombo",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChiTietCombos_MonAns_MaMonAn",
                        column: x => x.MaMonAn,
                        principalTable: "MonAns",
                        principalColumn: "MaMonAn",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DiaChiNguoiDungs",
                columns: table => new
                {
                    MaDiaChi = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenNguoiNhan = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SoDienThoai = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DiaChiCuThe = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    MaNguoiDung = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiaChiNguoiDungs", x => x.MaDiaChi);
                    table.ForeignKey(
                        name: "FK_DiaChiNguoiDungs_NguoiDungs_MaNguoiDung",
                        column: x => x.MaNguoiDung,
                        principalTable: "NguoiDungs",
                        principalColumn: "MaNguoiDung",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HoaDons",
                columns: table => new
                {
                    MaHoaDon = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NgayDat = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TongTien = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TrangThai = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DiaChiGiaoHang = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    MaNguoiDung = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HoaDons", x => x.MaHoaDon);
                    table.ForeignKey(
                        name: "FK_HoaDons_NguoiDungs_MaNguoiDung",
                        column: x => x.MaNguoiDung,
                        principalTable: "NguoiDungs",
                        principalColumn: "MaNguoiDung",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChiTietHoaDons",
                columns: table => new
                {
                    MaChiTiet = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SoLuong = table.Column<int>(type: "int", nullable: false),
                    DonGia = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MaHoaDon = table.Column<int>(type: "int", nullable: false),
                    MaMonAn = table.Column<int>(type: "int", nullable: true),
                    MaCombo = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietHoaDons", x => x.MaChiTiet);
                    table.ForeignKey(
                        name: "FK_ChiTietHoaDons_Combos_MaCombo",
                        column: x => x.MaCombo,
                        principalTable: "Combos",
                        principalColumn: "MaCombo");
                    table.ForeignKey(
                        name: "FK_ChiTietHoaDons_HoaDons_MaHoaDon",
                        column: x => x.MaHoaDon,
                        principalTable: "HoaDons",
                        principalColumn: "MaHoaDon",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChiTietHoaDons_MonAns_MaMonAn",
                        column: x => x.MaMonAn,
                        principalTable: "MonAns",
                        principalColumn: "MaMonAn");
                });

            migrationBuilder.InsertData(
                table: "CapThanhViens",
                columns: new[] { "MaCapThanhVien", "NguongDiem", "PhanTramGiamGia", "TenCap" },
                values: new object[,]
                {
                    { 1, 0, 0.0, "Đồng" },
                    { 2, 1000, 5.0, "Bạc" },
                    { 3, 5000, 10.0, "Vàng" }
                });

            migrationBuilder.InsertData(
                table: "Combos",
                columns: new[] { "MaCombo", "Gia", "HinhAnh", "MoTa", "TenCombo" },
                values: new object[,]
                {
                    { 1, 89000m, null, "1 Burger Zinger + 1 Khoai Tây Chiên (Vừa) + 1 Pepsi Lon", "Combo Zinger" },
                    { 2, 92000m, null, "2 Miếng Gà Rán + 1 Khoai Tây Chiên (Vừa) + 1 Pepsi Lon", "Combo Gà Rán 1 người" },
                    { 3, 199000m, null, "3 Miếng Gà Rán + 1 Burger Tôm + 2 Khoai Tây Chiên (Vừa) + 2 Pepsi Lon", "Combo Nhóm 2 người" },
                    { 4, 69000m, null, "1 Cơm Gà Rán + 1 Salad + 1 Pepsi Lon", "Combo Cơm Gà" }
                });

            migrationBuilder.InsertData(
                table: "LoaiMonAns",
                columns: new[] { "MaLoai", "TenLoai" },
                values: new object[,]
                {
                    { 1, "Gà Rán & Gà Quay" },
                    { 2, "Burger & Cơm" },
                    { 3, "Thức Ăn Nhẹ" },
                    { 4, "Tráng Miệng & Thức Uống" }
                });

            migrationBuilder.InsertData(
                table: "VaiTros",
                columns: new[] { "MaVaiTro", "TenVaiTro" },
                values: new object[,]
                {
                    { 1, "Admin" },
                    { 2, "Customer" }
                });

            migrationBuilder.InsertData(
                table: "MonAns",
                columns: new[] { "MaMonAn", "Gia", "HinhAnh", "IsNew", "IsPopular", "MaLoai", "MoTa", "OriginalPrice", "Rating", "ReviewCount", "TenMonAn" },
                values: new object[,]
                {
                    { 1, 36000m, null, false, true, 1, "Gà Rán Cay Hot & Spicy.", null, 4.7999999999999998, 250, "1 Miếng Gà Rán Hot & Spicy" },
                    { 2, 105000m, null, false, true, 1, "Combo 3 miếng Gà Rán Cay Hot & Spicy.", 108000m, 4.7000000000000002, 180, "3 Miếng Gà Rán Hot & Spicy" },
                    { 3, 69000m, null, true, false, 1, "Gà Quay Giấy Bạc đậm đà hương vị Việt.", null, 4.9000000000000004, 150, "1 Miếng Gà Quay Giấy Bạc" },
                    { 4, 55000m, null, false, true, 2, "Burger Zinger với 100% thịt phi lê gà cay giòn.", null, 4.9000000000000004, 310, "Burger Zinger" },
                    { 5, 45000m, null, false, false, 2, "Cơm Gà Rán kèm xà lách và sốt.", null, 4.5999999999999996, 120, "Cơm Gà Rán" },
                    { 6, 45000m, null, true, false, 2, "Burger nhân tôm tẩm bột chiên giòn, kết hợp với sốt đặc biệt.", null, 4.5999999999999996, 90, "Burger Tôm" },
                    { 7, 22000m, null, false, true, 3, "Khoai tây chiên giòn rụm, vàng óng.", null, 4.5, 450, "Khoai Tây Chiên (Vừa)" },
                    { 8, 42000m, null, false, false, 3, "5 miếng gà viên chiên giòn.", null, 4.4000000000000004, 130, "5 Gà Miếng Nuggets" },
                    { 9, 17000m, null, false, false, 4, "Nước ngọt có gas Pepsi mát lạnh sảng khoái.", null, 4.5, 100, "Pepsi Lon" },
                    { 10, 18000m, null, false, true, 4, "Bánh trứng nướng thơm lừng, béo ngậy.", null, 4.7999999999999998, 280, "Bánh Trứng" }
                });

            migrationBuilder.InsertData(
                table: "NguoiDungs",
                columns: new[] { "MaNguoiDung", "DiemTichLuy", "Email", "HoTen", "MaCapThanhVien", "MaVaiTro", "MatKhau", "NgaySinh", "SoDienThoai" },
                values: new object[] { 1, 0, "admin@example.com", "Admin", 1, 1, "$2a$11$gA9i54f.gYj02T/Iq0xV..36gT0zQL./C59Fh.Zk.K.dZ.zY.zY.O", new DateTime(1990, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "0123456789" });

            migrationBuilder.InsertData(
                table: "ChiTietCombos",
                columns: new[] { "MaCombo", "MaMonAn" },
                values: new object[,]
                {
                    { 1, 4 },
                    { 1, 7 },
                    { 1, 9 },
                    { 2, 1 },
                    { 2, 7 },
                    { 2, 9 },
                    { 3, 1 },
                    { 3, 6 },
                    { 3, 7 },
                    { 3, 9 },
                    { 4, 5 },
                    { 4, 9 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietCombos_MaMonAn",
                table: "ChiTietCombos",
                column: "MaMonAn");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietHoaDons_MaCombo",
                table: "ChiTietHoaDons",
                column: "MaCombo");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietHoaDons_MaHoaDon",
                table: "ChiTietHoaDons",
                column: "MaHoaDon");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietHoaDons_MaMonAn",
                table: "ChiTietHoaDons",
                column: "MaMonAn");

            migrationBuilder.CreateIndex(
                name: "IX_DiaChiNguoiDungs_MaNguoiDung",
                table: "DiaChiNguoiDungs",
                column: "MaNguoiDung");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDons_MaNguoiDung",
                table: "HoaDons",
                column: "MaNguoiDung");

            migrationBuilder.CreateIndex(
                name: "IX_MonAns_MaLoai",
                table: "MonAns",
                column: "MaLoai");

            migrationBuilder.CreateIndex(
                name: "IX_NguoiDungs_MaCapThanhVien",
                table: "NguoiDungs",
                column: "MaCapThanhVien");

            migrationBuilder.CreateIndex(
                name: "IX_NguoiDungs_MaVaiTro",
                table: "NguoiDungs",
                column: "MaVaiTro");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChiTietCombos");

            migrationBuilder.DropTable(
                name: "ChiTietHoaDons");

            migrationBuilder.DropTable(
                name: "DiaChiNguoiDungs");

            migrationBuilder.DropTable(
                name: "Combos");

            migrationBuilder.DropTable(
                name: "HoaDons");

            migrationBuilder.DropTable(
                name: "MonAns");

            migrationBuilder.DropTable(
                name: "NguoiDungs");

            migrationBuilder.DropTable(
                name: "LoaiMonAns");

            migrationBuilder.DropTable(
                name: "CapThanhViens");

            migrationBuilder.DropTable(
                name: "VaiTros");
        }
    }
}
