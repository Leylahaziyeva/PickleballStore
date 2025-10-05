using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PickleballStore.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ChangedSomeParameters : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "PackagedDate",
                table: "Orders",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ProcessingStartedDate",
                table: "Orders",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Addresses",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PackagedDate",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ProcessingStartedDate",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Addresses");
        }
    }
}
