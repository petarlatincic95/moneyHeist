using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoneyHeistAPI.Migrations
{
    public partial class AddingHeistStatusField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Heists",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "PLANNING");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Heists");
        }
    }
}
