using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FastFoodApp.Migrations
{
    /// <inheritdoc />
    public partial class RemoveDiaChiFromNguoiDung : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiaChi",
                table: "NguoiDungs");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DiaChi",
                table: "NguoiDungs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "NguoiDungs",
                keyColumn: "MaNguoiDung",
                keyValue: 1,
                column: "DiaChi",
                value: "123 Admin Street");
        }
    }
}
