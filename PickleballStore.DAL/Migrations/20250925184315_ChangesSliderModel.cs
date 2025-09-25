using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PickleballStore.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ChangesSliderModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "Sliders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sliders_ProductId",
                table: "Sliders",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sliders_Products_ProductId",
                table: "Sliders",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sliders_Products_ProductId",
                table: "Sliders");

            migrationBuilder.DropIndex(
                name: "IX_Sliders_ProductId",
                table: "Sliders");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "Sliders");
        }
    }
}
