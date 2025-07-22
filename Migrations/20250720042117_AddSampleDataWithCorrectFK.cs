using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FastFoodApp.Migrations
{
    /// <inheritdoc />
    public partial class AddSampleDataWithCorrectFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "HinhAnh",
                table: "MonAns",
                type: "varbinary(max)",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)");

            migrationBuilder.AlterColumn<byte[]>(
                name: "HinhAnh",
                table: "Combos",
                type: "varbinary(max)",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)");

            migrationBuilder.InsertData(
                table: "Combos",
                columns: new[] { "MaCombo", "Gia", "HinhAnh", "MoTa", "TenCombo" },
                values: new object[,]
                {
                    { 1, 65000m, null, "Bao gồm 1 Gà Rán Truyền Thống, 1 Khoai Tây Chiên (Vừa), 1 Pepsi Lon.", "Combo Gà Rán A" },
                    { 2, 85000m, null, "Bao gồm 1 Burger Zinger, 1 Khoai Tây Chiên (Vừa), 1 Pepsi Lon.", "Combo Burger Zinger" },
                    { 3, 159000m, null, "Bao gồm 3 Gà Rán Truyền Thống, 2 Khoai Tây Chiên (Vừa), 2 Pepsi Lon.", "Combo Nhóm Vui Vẻ" }
                });

            migrationBuilder.InsertData(
                table: "LoaiMonAns",
                columns: new[] { "MaLoai", "TenLoai" },
                values: new object[] { 5, "Món Ăn Kèm" });

            migrationBuilder.InsertData(
                table: "MonAns",
                columns: new[] { "MaMonAn", "Gia", "HinhAnh", "MaLoai", "MoTa", "TenMonAn" },
                values: new object[,]
                {
                    { 1, 35000m, null, 1, "Gà rán giòn tan với công thức 11 loại thảo mộc và gia vị.", "1 Miếng Gà Rán Truyền Thống" },
                    { 2, 36000m, null, 1, "Gà rán cay nồng, đậm vị cho người thích ăn cay.", "1 Miếng Gà Rán Cay" },
                    { 3, 55000m, null, 3, "Burger với miếng phi lê gà cay giòn, rau diếp tươi và sốt mayonnaise.", "Burger Zinger" },
                    { 4, 45000m, null, 3, "Burger nhân tôm tẩm bột chiên giòn, kết hợp với sốt đặc biệt.", "Burger Tôm" },
                    { 6, 15000m, null, 4, "Nước ngọt có gas Pepsi mát lạnh.", "Pepsi Lon" },
                    { 7, 15000m, null, 4, "Nước ngọt có gas 7 Up vị chanh.", "7 Up Lon" }
                });

            migrationBuilder.UpdateData(
                table: "NguoiDungs",
                keyColumn: "MaNguoiDung",
                keyValue: 1,
                column: "MatKhau",
                value: "$2a$11$gA9i54f.gYj02T/Iq0xV..36gT0zQL./C59Fh.Zk.K.dZ.zY.zY.O");

            migrationBuilder.InsertData(
                table: "ChiTietCombos",
                columns: new[] { "MaCombo", "MaMonAn" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 1, 6 },
                    { 2, 3 },
                    { 2, 6 },
                    { 3, 2 },
                    { 3, 7 }
                });

            migrationBuilder.InsertData(
                table: "MonAns",
                columns: new[] { "MaMonAn", "Gia", "HinhAnh", "MaLoai", "MoTa", "TenMonAn" },
                values: new object[] { 5, 20000m, null, 5, "Khoai tây chiên giòn rụm, vàng óng.", "Khoai Tây Chiên (Vừa)" });

            migrationBuilder.InsertData(
                table: "ChiTietCombos",
                columns: new[] { "MaCombo", "MaMonAn" },
                values: new object[,]
                {
                    { 1, 5 },
                    { 2, 5 },
                    { 3, 5 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ChiTietCombos",
                keyColumns: new[] { "MaCombo", "MaMonAn" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "ChiTietCombos",
                keyColumns: new[] { "MaCombo", "MaMonAn" },
                keyValues: new object[] { 1, 5 });

            migrationBuilder.DeleteData(
                table: "ChiTietCombos",
                keyColumns: new[] { "MaCombo", "MaMonAn" },
                keyValues: new object[] { 1, 6 });

            migrationBuilder.DeleteData(
                table: "ChiTietCombos",
                keyColumns: new[] { "MaCombo", "MaMonAn" },
                keyValues: new object[] { 2, 3 });

            migrationBuilder.DeleteData(
                table: "ChiTietCombos",
                keyColumns: new[] { "MaCombo", "MaMonAn" },
                keyValues: new object[] { 2, 5 });

            migrationBuilder.DeleteData(
                table: "ChiTietCombos",
                keyColumns: new[] { "MaCombo", "MaMonAn" },
                keyValues: new object[] { 2, 6 });

            migrationBuilder.DeleteData(
                table: "ChiTietCombos",
                keyColumns: new[] { "MaCombo", "MaMonAn" },
                keyValues: new object[] { 3, 2 });

            migrationBuilder.DeleteData(
                table: "ChiTietCombos",
                keyColumns: new[] { "MaCombo", "MaMonAn" },
                keyValues: new object[] { 3, 5 });

            migrationBuilder.DeleteData(
                table: "ChiTietCombos",
                keyColumns: new[] { "MaCombo", "MaMonAn" },
                keyValues: new object[] { 3, 7 });

            migrationBuilder.DeleteData(
                table: "MonAns",
                keyColumn: "MaMonAn",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Combos",
                keyColumn: "MaCombo",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Combos",
                keyColumn: "MaCombo",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Combos",
                keyColumn: "MaCombo",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "MonAns",
                keyColumn: "MaMonAn",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "MonAns",
                keyColumn: "MaMonAn",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "MonAns",
                keyColumn: "MaMonAn",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "MonAns",
                keyColumn: "MaMonAn",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "MonAns",
                keyColumn: "MaMonAn",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "MonAns",
                keyColumn: "MaMonAn",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "LoaiMonAns",
                keyColumn: "MaLoai",
                keyValue: 5);

            migrationBuilder.AlterColumn<byte[]>(
                name: "HinhAnh",
                table: "MonAns",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0],
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte[]>(
                name: "HinhAnh",
                table: "Combos",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0],
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "NguoiDungs",
                keyColumn: "MaNguoiDung",
                keyValue: 1,
                column: "MatKhau",
                value: "$2a$11$...");
        }
    }
}
