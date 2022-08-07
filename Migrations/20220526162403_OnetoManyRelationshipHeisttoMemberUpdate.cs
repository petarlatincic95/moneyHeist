using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoneyHeistAPI.Migrations
{
    public partial class OnetoManyRelationshipHeisttoMemberUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "HeistId",
                table: "HeistMembers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_HeistMembers_HeistId",
                table: "HeistMembers",
                column: "HeistId");

            migrationBuilder.AddForeignKey(
                name: "FK_HeistMembers_Heists_HeistId",
                table: "HeistMembers",
                column: "HeistId",
                principalTable: "Heists",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HeistMembers_Heists_HeistId",
                table: "HeistMembers");

            migrationBuilder.DropIndex(
                name: "IX_HeistMembers_HeistId",
                table: "HeistMembers");

            migrationBuilder.DropColumn(
                name: "HeistId",
                table: "HeistMembers");
        }
    }
}
