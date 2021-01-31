using Microsoft.EntityFrameworkCore.Migrations;

namespace eVotingApi.Migrations
{
    public partial class revert : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Candidate_Member_MemberId1",
                table: "Candidate");

            migrationBuilder.DropIndex(
                name: "IX_Candidate_MemberId1",
                table: "Candidate");

            migrationBuilder.DropColumn(
                name: "MemberId1",
                table: "Candidate");

            migrationBuilder.AddForeignKey(
                name: "FK_Candidate_Member_MemberId",
                table: "Candidate",
                column: "MemberId",
                principalTable: "Member",
                principalColumn: "MemberId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Candidate_Member_MemberId",
                table: "Candidate");

            migrationBuilder.AddColumn<long>(
                name: "MemberId1",
                table: "Candidate",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Candidate_MemberId1",
                table: "Candidate",
                column: "MemberId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Candidate_Member_MemberId1",
                table: "Candidate",
                column: "MemberId1",
                principalTable: "Member",
                principalColumn: "MemberId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
