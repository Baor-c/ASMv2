using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FastFoodApp.Migrations
{
    /// <inheritdoc />
    public partial class AddAttributesToMonAn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "LoaiMonAns",
                keyColumn: "MaLoai",
                keyValue: 5);

            migrationBuilder.AddColumn<bool>(
                name: "IsNew",
                table: "MonAns",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPopular",
                table: "MonAns",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "OriginalPrice",
                table: "MonAns",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Rating",
                table: "MonAns",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "ReviewCount",
                table: "MonAns",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "LoaiMonAns",
                keyColumn: "MaLoai",
                keyValue: 2,
                column: "TenLoai",
                value: "Burger");

            migrationBuilder.UpdateData(
                table: "LoaiMonAns",
                keyColumn: "MaLoai",
                keyValue: 3,
                column: "TenLoai",
                value: "Nước Uống");

            migrationBuilder.UpdateData(
                table: "LoaiMonAns",
                keyColumn: "MaLoai",
                keyValue: 4,
                column: "TenLoai",
                value: "Món Ăn Kèm");

            migrationBuilder.UpdateData(
                table: "MonAns",
                keyColumn: "MaMonAn",
                keyValue: 1,
                columns: new[] { "IsNew", "IsPopular", "OriginalPrice", "Rating", "ReviewCount" },
                values: new object[] { true, true, 40000m, 4.7999999999999998, 152 });

            migrationBuilder.UpdateData(
                table: "MonAns",
                keyColumn: "MaMonAn",
                keyValue: 2,
                columns: new[] { "IsNew", "IsPopular", "OriginalPrice", "Rating", "ReviewCount" },
                values: new object[] { false, true, null, 4.7000000000000002, 130 });

            migrationBuilder.UpdateData(
                table: "MonAns",
                keyColumn: "MaMonAn",
                keyValue: 3,
                columns: new[] { "IsNew", "IsPopular", "OriginalPrice", "Rating", "ReviewCount" },
                values: new object[] { false, true, 65000m, 4.9000000000000004, 210 });

            migrationBuilder.UpdateData(
                table: "MonAns",
                keyColumn: "MaMonAn",
                keyValue: 4,
                columns: new[] { "IsNew", "IsPopular", "OriginalPrice", "Rating", "ReviewCount" },
                values: new object[] { true, false, null, 4.5999999999999996, 88 });

            migrationBuilder.UpdateData(
                table: "MonAns",
                keyColumn: "MaMonAn",
                keyValue: 5,
                columns: new[] { "IsNew", "IsPopular", "OriginalPrice", "Rating", "ReviewCount" },
                values: new object[] { false, true, null, 4.5, 350 });

            migrationBuilder.UpdateData(
                table: "MonAns",
                keyColumn: "MaMonAn",
                keyValue: 6,
                columns: new[] { "IsNew", "IsPopular", "OriginalPrice", "Rating", "ReviewCount" },
                values: new object[] { false, false, null, 4.5, 100 });

            migrationBuilder.UpdateData(
                table: "MonAns",
                keyColumn: "MaMonAn",
                keyValue: 7,
                columns: new[] { "IsNew", "IsPopular", "OriginalPrice", "Rating", "ReviewCount" },
                values: new object[] { false, false, null, 4.5, 100 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsNew",
                table: "MonAns");

            migrationBuilder.DropColumn(
                name: "IsPopular",
                table: "MonAns");

            migrationBuilder.DropColumn(
                name: "OriginalPrice",
                table: "MonAns");

            migrationBuilder.DropColumn(
                name: "Rating",
                table: "MonAns");

            migrationBuilder.DropColumn(
                name: "ReviewCount",
                table: "MonAns");

            migrationBuilder.UpdateData(
                table: "LoaiMonAns",
                keyColumn: "MaLoai",
                keyValue: 2,
                column: "TenLoai",
                value: "Pizza");

            migrationBuilder.UpdateData(
                table: "LoaiMonAns",
                keyColumn: "MaLoai",
                keyValue: 3,
                column: "TenLoai",
                value: "Burger");

            migrationBuilder.UpdateData(
                table: "LoaiMonAns",
                keyColumn: "MaLoai",
                keyValue: 4,
                column: "TenLoai",
                value: "Nước Uống");

            migrationBuilder.InsertData(
                table: "LoaiMonAns",
                columns: new[] { "MaLoai", "TenLoai" },
                values: new object[] { 5, "Món Ăn Kèm" });
        }
    }
}
