using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FastFoodApp.Migrations
{
    /// <inheritdoc />
    public partial class AddPhuongThucThanhToanColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartItemSelectedOptions");

            migrationBuilder.DropTable(
                name: "ProductOptionMappings");

            migrationBuilder.DropTable(
                name: "Options");

            migrationBuilder.DropTable(
                name: "OptionGroups");

            migrationBuilder.AddColumn<string>(
                name: "PhuongThucThanhToan",
                table: "HoaDons",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhuongThucThanhToan",
                table: "HoaDons");

            migrationBuilder.CreateTable(
                name: "OptionGroups",
                columns: table => new
                {
                    OptionGroupID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SelectionType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OptionGroups", x => x.OptionGroupID);
                });

            migrationBuilder.CreateTable(
                name: "Options",
                columns: table => new
                {
                    OptionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OptionGroupID = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PriceAdjustment = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Options", x => x.OptionID);
                    table.ForeignKey(
                        name: "FK_Options_OptionGroups_OptionGroupID",
                        column: x => x.OptionGroupID,
                        principalTable: "OptionGroups",
                        principalColumn: "OptionGroupID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductOptionMappings",
                columns: table => new
                {
                    ProductID = table.Column<int>(type: "int", nullable: false),
                    OptionGroupID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductOptionMappings", x => new { x.ProductID, x.OptionGroupID });
                    table.ForeignKey(
                        name: "FK_ProductOptionMappings_MonAns_ProductID",
                        column: x => x.ProductID,
                        principalTable: "MonAns",
                        principalColumn: "MaMonAn",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductOptionMappings_OptionGroups_OptionGroupID",
                        column: x => x.OptionGroupID,
                        principalTable: "OptionGroups",
                        principalColumn: "OptionGroupID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CartItemSelectedOptions",
                columns: table => new
                {
                    CartItemID = table.Column<int>(type: "int", nullable: false),
                    OptionID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItemSelectedOptions", x => new { x.CartItemID, x.OptionID });
                    table.ForeignKey(
                        name: "FK_CartItemSelectedOptions_CartItems_CartItemID",
                        column: x => x.CartItemID,
                        principalTable: "CartItems",
                        principalColumn: "CartItemID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartItemSelectedOptions_Options_OptionID",
                        column: x => x.OptionID,
                        principalTable: "Options",
                        principalColumn: "OptionID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CartItemSelectedOptions_OptionID",
                table: "CartItemSelectedOptions",
                column: "OptionID");

            migrationBuilder.CreateIndex(
                name: "IX_Options_OptionGroupID",
                table: "Options",
                column: "OptionGroupID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductOptionMappings_OptionGroupID",
                table: "ProductOptionMappings",
                column: "OptionGroupID");
        }
    }
}
