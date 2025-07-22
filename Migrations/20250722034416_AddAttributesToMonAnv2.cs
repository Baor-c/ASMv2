using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FastFoodApp.Migrations
{
    /// <inheritdoc />
    public partial class AddAttributesToMonAnv2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "MonAns",
                keyColumn: "MaMonAn",
                keyValue: 3,
                column: "MaLoai",
                value: 2);

            migrationBuilder.UpdateData(
                table: "MonAns",
                keyColumn: "MaMonAn",
                keyValue: 4,
                column: "MaLoai",
                value: 2);

            migrationBuilder.UpdateData(
                table: "MonAns",
                keyColumn: "MaMonAn",
                keyValue: 5,
                column: "MaLoai",
                value: 4);

            migrationBuilder.UpdateData(
                table: "MonAns",
                keyColumn: "MaMonAn",
                keyValue: 6,
                column: "MaLoai",
                value: 3);

            migrationBuilder.UpdateData(
                table: "MonAns",
                keyColumn: "MaMonAn",
                keyValue: 7,
                column: "MaLoai",
                value: 3);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "MonAns",
                keyColumn: "MaMonAn",
                keyValue: 3,
                column: "MaLoai",
                value: 3);

            migrationBuilder.UpdateData(
                table: "MonAns",
                keyColumn: "MaMonAn",
                keyValue: 4,
                column: "MaLoai",
                value: 3);

            migrationBuilder.UpdateData(
                table: "MonAns",
                keyColumn: "MaMonAn",
                keyValue: 5,
                column: "MaLoai",
                value: 5);

            migrationBuilder.UpdateData(
                table: "MonAns",
                keyColumn: "MaMonAn",
                keyValue: 6,
                column: "MaLoai",
                value: 4);

            migrationBuilder.UpdateData(
                table: "MonAns",
                keyColumn: "MaMonAn",
                keyValue: 7,
                column: "MaLoai",
                value: 4);
        }
    }
}
