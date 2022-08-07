using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoneyHeistAPI.Migrations
{
    public partial class EmailUniqueMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HeistMemberMemberSkill_HeistMembers_HeistMembersId",
                table: "HeistMemberMemberSkill");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HeistMembers",
                table: "HeistMembers");

            migrationBuilder.RenameTable(
                name: "HeistMembers",
                newName: "HeistMember");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "HeistMember",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HeistMember",
                table: "HeistMember",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_HeistMember_Email",
                table: "HeistMember",
                column: "Email",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_HeistMemberMemberSkill_HeistMember_HeistMembersId",
                table: "HeistMemberMemberSkill",
                column: "HeistMembersId",
                principalTable: "HeistMember",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HeistMemberMemberSkill_HeistMember_HeistMembersId",
                table: "HeistMemberMemberSkill");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HeistMember",
                table: "HeistMember");

            migrationBuilder.DropIndex(
                name: "IX_HeistMember_Email",
                table: "HeistMember");

            migrationBuilder.RenameTable(
                name: "HeistMember",
                newName: "HeistMembers");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "HeistMembers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HeistMembers",
                table: "HeistMembers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_HeistMemberMemberSkill_HeistMembers_HeistMembersId",
                table: "HeistMemberMemberSkill",
                column: "HeistMembersId",
                principalTable: "HeistMembers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
