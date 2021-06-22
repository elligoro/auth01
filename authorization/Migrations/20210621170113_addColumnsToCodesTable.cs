using Microsoft.EntityFrameworkCore.Migrations;

namespace authorization.Migrations
{
    public partial class addColumnsToCodesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AccessToken",
                table: "Codes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RefreshToken",
                table: "Codes",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccessToken",
                table: "Codes");

            migrationBuilder.DropColumn(
                name: "RefreshToken",
                table: "Codes");
        }
    }
}
