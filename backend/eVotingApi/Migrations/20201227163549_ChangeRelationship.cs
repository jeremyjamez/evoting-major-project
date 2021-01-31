using Microsoft.EntityFrameworkCore.Migrations;

namespace eVotingApi.Migrations
{
    public partial class ChangeRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Constituency_Candidate_MemberId",
                table: "Constituency");

            migrationBuilder.DropIndex(
                name: "IX_Constituency_MemberId",
                table: "Constituency");

            migrationBuilder.CreateIndex(
                name: "IX_Candidate_ConstituencyId",
                table: "Candidate",
                column: "ConstituencyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Candidate_Constituency_ConstituencyId",
                table: "Candidate",
                column: "ConstituencyId",
                principalTable: "Constituency",
                principalColumn: "ConstituencyId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Candidate_Constituency_ConstituencyId",
                table: "Candidate");

            migrationBuilder.DropIndex(
                name: "IX_Candidate_ConstituencyId",
                table: "Candidate");

            migrationBuilder.CreateIndex(
                name: "IX_Constituency_MemberId",
                table: "Constituency",
                column: "MemberId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Constituency_Candidate_MemberId",
                table: "Constituency",
                column: "MemberId",
                principalTable: "Candidate",
                principalColumn: "MemberId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
