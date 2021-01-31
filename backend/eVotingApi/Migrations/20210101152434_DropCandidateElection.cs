using Microsoft.EntityFrameworkCore.Migrations;

namespace eVotingApi.Migrations
{
    public partial class DropCandidateElection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ElectionId",
                table: "Candidate",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Candidate_ElectionId",
                table: "Candidate",
                column: "ElectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Candidate_Election_ElectionId",
                table: "Candidate",
                column: "ElectionId",
                principalTable: "Election",
                principalColumn: "ElectionId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Candidate_Election_ElectionId",
                table: "Candidate");

            migrationBuilder.DropIndex(
                name: "IX_Candidate_ElectionId",
                table: "Candidate");

            migrationBuilder.DropColumn(
                name: "ElectionId",
                table: "Candidate");

            migrationBuilder.CreateTable(
                name: "CandidateElection",
                columns: table => new
                {
                    CandidateId = table.Column<long>(type: "bigint", nullable: false),
                    ElectionId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CandidateElection", x => new { x.CandidateId, x.ElectionId });
                    table.ForeignKey(
                        name: "FK_CandidateElection_Candidate_CandidateId",
                        column: x => x.CandidateId,
                        principalTable: "Candidate",
                        principalColumn: "CandidateId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CandidateElection_Election_ElectionId",
                        column: x => x.ElectionId,
                        principalTable: "Election",
                        principalColumn: "ElectionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CandidateElection_ElectionId",
                table: "CandidateElection",
                column: "ElectionId");
        }
    }
}
