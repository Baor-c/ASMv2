using Microsoft.EntityFrameworkCore.Migrations;

namespace FastFoodApp.Migrations
{
    public partial class AddPhuongThucThanhToanManual : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add PhuongThucThanhToan column to HoaDon table if it doesn't exist
            migrationBuilder.Sql(@"
                IF NOT EXISTS (
                    SELECT 1
                    FROM sys.columns
                    WHERE name = 'PhuongThucThanhToan'
                    AND object_id = OBJECT_ID('HoaDons')
                )
                BEGIN
                    ALTER TABLE [HoaDons]
                    ADD [PhuongThucThanhToan] nvarchar(50) NULL;
                END
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove the column if needed
            migrationBuilder.Sql(@"
                IF EXISTS (
                    SELECT 1
                    FROM sys.columns
                    WHERE name = 'PhuongThucThanhToan'
                    AND object_id = OBJECT_ID('HoaDons')
                )
                BEGIN
                    ALTER TABLE [HoaDons]
                    DROP COLUMN [PhuongThucThanhToan];
                END
            ");
        }
    }
}
