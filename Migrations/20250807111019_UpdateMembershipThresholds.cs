using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FastFoodApp.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMembershipThresholds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "CapThanhViens",
                keyColumn: "MaCapThanhVien",
                keyValue: 2,
                column: "NguongDiem",
                value: 300);

            migrationBuilder.UpdateData(
                table: "CapThanhViens",
                keyColumn: "MaCapThanhVien",
                keyValue: 3,
                column: "NguongDiem",
                value: 500);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "CapThanhViens",
                keyColumn: "MaCapThanhVien",
                keyValue: 2,
                column: "NguongDiem",
                value: 1000);

            migrationBuilder.UpdateData(
                table: "CapThanhViens",
                keyColumn: "MaCapThanhVien",
                keyValue: 3,
                column: "NguongDiem",
                value: 5000);
        }
    }
}
