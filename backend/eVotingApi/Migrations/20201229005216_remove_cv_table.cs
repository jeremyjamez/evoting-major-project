using Microsoft.EntityFrameworkCore.Migrations;

namespace eVotingApi.Migrations
{
    public partial class remove_cv_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Candidate_Member_MemberId",
                table: "Candidate");

            migrationBuilder.DropTable(
                name: "Candidate_Vote");

            migrationBuilder.RenameColumn(
                name: "MemberId",
                table: "Candidate",
                newName: "CandidateId");

            migrationBuilder.AddColumn<long>(
                name: "ElectionId",
                table: "Candidate",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vote_CandidateId",
                table: "Vote",
                column: "CandidateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Candidate_Member_CandidateId",
                table: "Candidate",
                column: "CandidateId",
                principalTable: "Member",
                principalColumn: "MemberId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Vote_Candidate_CandidateId",
                table: "Vote",
                column: "CandidateId",
                principalTable: "Candidate",
                principalColumn: "CandidateId",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Candidate_Member_CandidateId",
                table: "Candidate");

            migrationBuilder.DropForeignKey(
                name: "FK_Vote_Candidate_CandidateId",
                table: "Vote");

            migrationBuilder.DropIndex(
                name: "IX_Vote_CandidateId",
                table: "Vote");

            migrationBuilder.DropColumn(
                name: "ElectionId",
                table: "Candidate");

            migrationBuilder.RenameColumn(
                name: "CandidateId",
                table: "Candidate",
                newName: "MemberId");

            migrationBuilder.CreateTable(
                name: "Candidate_Vote",
                columns: table => new
                {
                    ConstituencyId = table.Column<long>(type: "bigint", nullable: false),
                    CandidateId = table.Column<long>(type: "bigint", nullable: false),
                    ElectionId = table.Column<long>(type: "bigint", nullable: false),
                    VoteCount = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Candidate_Vote", x => new { x.ConstituencyId, x.CandidateId });
                    table.ForeignKey(
                        name: "FK_Candidate_Vote_Candidate_CandidateId",
                        column: x => x.CandidateId,
                        principalTable: "Candidate",
                        principalColumn: "MemberId");
                    table.ForeignKey(
                        name: "FK_Candidate_Vote_Constituency_ConstituencyId",
                        column: x => x.ConstituencyId,
                        principalTable: "Constituency",
                        principalColumn: "ConstituencyId");
                    table.ForeignKey(
                        name: "FK_Candidate_Vote_Election_ElectionId",
                        column: x => x.ElectionId,
                        principalTable: "Election",
                        principalColumn: "ElectionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Candidate_Vote_CandidateId",
                table: "Candidate_Vote",
                column: "CandidateId");

            migrationBuilder.CreateIndex(
                name: "IX_Candidate_Vote_ElectionId",
                table: "Candidate_Vote",
                column: "ElectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Candidate_Member_MemberId",
                table: "Candidate",
                column: "MemberId",
                principalTable: "Member",
                principalColumn: "MemberId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
