using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppSec__practicalAssignment_.Migrations
{
    /// <inheritdoc />
    public partial class RemoveMobileNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MobileNo",
                table: "AspNetUsers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MobileNo",
                table: "AspNetUsers",
                type: "int",
                nullable: true);
        }
    }
}
