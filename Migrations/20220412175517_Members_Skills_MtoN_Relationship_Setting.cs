using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoneyHeistAPI.Migrations
{
    public partial class Members_Skills_MtoN_Relationship_Setting : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HeistMemberMemberSkill",
                columns: table => new
                {
                    HeistMembersId = table.Column<int>(type: "int", nullable: false),
                    MemberSkillsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HeistMemberMemberSkill", x => new { x.HeistMembersId, x.MemberSkillsId });
                    table.ForeignKey(
                        name: "FK_HeistMemberMemberSkill_HeistMembers_HeistMembersId",
                        column: x => x.HeistMembersId,
                        principalTable: "HeistMembers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HeistMemberMemberSkill_MemberSkills_MemberSkillsId",
                        column: x => x.MemberSkillsId,
                        principalTable: "MemberSkills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HeistMemberMemberSkill_MemberSkillsId",
                table: "HeistMemberMemberSkill",
                column: "MemberSkillsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HeistMemberMemberSkill");
        }
    }
}
