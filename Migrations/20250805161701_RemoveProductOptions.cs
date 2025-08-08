using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FastFoodApp.Migrations
{
    /// <inheritdoc />
    public partial class RemoveProductOptions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop foreign key constraints first
            migrationBuilder.DropForeignKey(
                name: "FK_CartItemSelectedOptions_CartItems_CartItemID",
                table: "CartItemSelectedOptions");

            migrationBuilder.DropForeignKey(
                name: "FK_CartItemSelectedOptions_Options_OptionID",
                table: "CartItemSelectedOptions");

            migrationBuilder.DropForeignKey(
                name: "FK_Options_OptionGroups_OptionGroupID",
                table: "Options");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductOptionMappings_MonAns_ProductID",
                table: "ProductOptionMappings");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductOptionMappings_OptionGroups_OptionGroupID",
                table: "ProductOptionMappings");

            // Drop tables in order
            migrationBuilder.DropTable(
                name: "CartItemSelectedOptions");

            migrationBuilder.DropTable(
                name: "ProductOptionMappings");

            migrationBuilder.DropTable(
                name: "Options");

            migrationBuilder.DropTable(
                name: "OptionGroups");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // We cannot recreate these tables as we are removing the models
            // This migration is one-way and cannot be reverted
        }
    }
}
