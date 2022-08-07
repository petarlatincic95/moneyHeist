using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoneyHeistAPI.Migrations
{
    public partial class HeistHeistSkill : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Heists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartTime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EndTime = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Heists", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HeistSkills",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SkillName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SkillLevel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RequiredMembers = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HeistSkills", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HeistHeistSkill",
                columns: table => new
                {
                    HeistSkillsId = table.Column<int>(type: "int", nullable: false),
                    HeistsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HeistHeistSkill", x => new { x.HeistSkillsId, x.HeistsId });
                    table.ForeignKey(
                        name: "FK_HeistHeistSkill_Heists_HeistsId",
                        column: x => x.HeistsId,
                        principalTable: "Heists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HeistHeistSkill_HeistSkills_HeistSkillsId",
                        column: x => x.HeistSkillsId,
                        principalTable: "HeistSkills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HeistHeistSkill_HeistsId",
                table: "HeistHeistSkill",
                column: "HeistsId");

            migrationBuilder.CreateIndex(
                name: "IX_Heists_Name",
                table: "Heists",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HeistHeistSkill");

            migrationBuilder.DropTable(
                name: "Heists");

            migrationBuilder.DropTable(
                name: "HeistSkills");
        }
    }
}
