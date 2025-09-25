using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PickleballStore.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ProductVariantNameChanged : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageName",
                table: "ProductVariants",
                newName: "OptionImageName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OptionImageName",
                table: "ProductVariants",
                newName: "ImageName");
        }
    }
}
