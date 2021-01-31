using Microsoft.EntityFrameworkCore.Migrations;

namespace eVotingApi.Migrations
{
    public partial class lotsofthings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Colour",
                table: "PoliticalParty",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Logo",
                table: "PoliticalParty",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Photo",
                table: "Member",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "MemberOfParliament",
                columns: table => new
                {
                    ConstituencyId = table.Column<long>(type: "bigint", nullable: false),
                    CandidateId = table.Column<long>(type: "bigint", nullable: false),
                    MinisterOf = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberOfParliament", x => new { x.ConstituencyId, x.CandidateId });
                    table.ForeignKey(
                        name: "FK_MemberOfParliament_Candidate_CandidateId",
                        column: x => x.CandidateId,
                        principalTable: "Candidate",
                        principalColumn: "CandidateId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_MemberOfParliament_Constituency_ConstituencyId",
                        column: x => x.ConstituencyId,
                        principalTable: "Constituency",
                        principalColumn: "ConstituencyId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MemberOfParliament_CandidateId",
                table: "MemberOfParliament",
                column: "CandidateId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MemberOfParliament_ConstituencyId",
                table: "MemberOfParliament",
                column: "ConstituencyId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MemberOfParliament");

            migrationBuilder.DropColumn(
                name: "Colour",
                table: "PoliticalParty");

            migrationBuilder.DropColumn(
                name: "Logo",
                table: "PoliticalParty");

            migrationBuilder.DropColumn(
                name: "Photo",
                table: "Member");
        }
    }
}
