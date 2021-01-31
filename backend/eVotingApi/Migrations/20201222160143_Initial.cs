using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eVotingApi.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Constituency",
                columns: table => new
                {
                    ConstituencyId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RegisteredVotersCount = table.Column<int>(type: "int", nullable: false),
                    CandidateId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Constituency", x => x.ConstituencyId);
                    table.ForeignKey(
                        name: "FK_Constituency_Candidate",
                        column: x => x.CandidateId,
                        principalTable: "Candidate",
                        principalColumn: "CandidateId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Election",
                columns: table => new
                {
                    ElectionId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ElectionType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ElectionDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Election", x => x.ElectionId);
                });

            migrationBuilder.CreateTable(
                name: "Candidate",
                columns: table => new
                {
                    CandidateId = table.Column<long>(type: "bigint", nullable: false),
                    Prefix = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MiddleName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Suffix = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Affiliation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConstituencyId = table.Column<long>(type: "bigint", nullable: false),
                    Parish = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Candidate", x => x.CandidateId);
                    table.ForeignKey(
                        name: "FK_Candidate_Constituency_CandidateId",
                        column: x => x.CandidateId,
                        principalTable: "Constituency",
                        principalColumn: "ConstituencyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PollingDivision",
                columns: table => new
                {
                    DivisionId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ConstituencyId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PollingDivision", x => x.DivisionId);
                    table.ForeignKey(
                        name: "FK_PollingDivision_Constituency_ConstituencyId",
                        column: x => x.ConstituencyId,
                        principalTable: "Constituency",
                        principalColumn: "ConstituencyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Voter",
                columns: table => new
                {
                    VoterId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Prefix = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MiddleName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Telephone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Occupation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConstituencyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Voter", x => x.VoterId);
                    table.ForeignKey(
                        name: "FK_Voter_Constituency_ConstituencyId",
                        column: x => x.ConstituencyId,
                        principalTable: "Constituency",
                        principalColumn: "ConstituencyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Candidate_Vote",
                columns: table => new
                {
                    ConstituencyId = table.Column<long>(type: "bigint", nullable: false),
                    CandidateId = table.Column<long>(type: "bigint", nullable: false),
                    VoteCount = table.Column<long>(type: "bigint", nullable: false),
                    ElectionId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Candidate_Vote", x => new { x.ConstituencyId, x.CandidateId });
                    table.ForeignKey(
                        name: "FK_Candidate_Vote_Candidate_CandidateId",
                        column: x => x.CandidateId,
                        principalTable: "Candidate",
                        principalColumn: "CandidateId");
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

            migrationBuilder.CreateTable(
                name: "PollingCentre",
                columns: table => new
                {
                    CentreId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DivisionId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PollingDivisionDivisionId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PollingCentre", x => x.CentreId);
                    table.ForeignKey(
                        name: "FK_PollingCentre_PollingDivision_PollingDivisionDivisionId",
                        column: x => x.PollingDivisionDivisionId,
                        principalTable: "PollingDivision",
                        principalColumn: "DivisionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Vote",
                columns: table => new
                {
                    VoteId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VoterId = table.Column<long>(type: "bigint", nullable: false),
                    CandidateId = table.Column<long>(type: "bigint", nullable: false),
                    ElectionId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vote", x => x.VoteId);
                    table.ForeignKey(
                        name: "FK_Vote_Election_ElectionId",
                        column: x => x.ElectionId,
                        principalTable: "Election",
                        principalColumn: "ElectionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Vote_Voter_VoterId",
                        column: x => x.VoterId,
                        principalTable: "Voter",
                        principalColumn: "VoterId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PollingStation",
                columns: table => new
                {
                    StationId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StationNumber = table.Column<int>(type: "int", nullable: false),
                    CentreId = table.Column<long>(type: "bigint", nullable: false),
                    PollingCentreCentreId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PollingStation", x => x.StationId);
                    table.ForeignKey(
                        name: "FK_PollingStation_PollingCentre_PollingCentreCentreId",
                        column: x => x.PollingCentreCentreId,
                        principalTable: "PollingCentre",
                        principalColumn: "CentreId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Election",
                columns: new[] { "ElectionId", "ElectionDate", "ElectionType" },
                values: new object[] { 1L, new DateTime(2020, 12, 22, 11, 1, 42, 633, DateTimeKind.Local).AddTicks(892), "General" });

            migrationBuilder.CreateIndex(
                name: "IX_Candidate_Vote_CandidateId",
                table: "Candidate_Vote",
                column: "CandidateId");

            migrationBuilder.CreateIndex(
                name: "IX_Candidate_Vote_ElectionId",
                table: "Candidate_Vote",
                column: "ElectionId");

            migrationBuilder.CreateIndex(
                name: "IX_PollingCentre_PollingDivisionDivisionId",
                table: "PollingCentre",
                column: "PollingDivisionDivisionId");

            migrationBuilder.CreateIndex(
                name: "IX_PollingDivision_ConstituencyId",
                table: "PollingDivision",
                column: "ConstituencyId");

            migrationBuilder.CreateIndex(
                name: "IX_PollingStation_PollingCentreCentreId",
                table: "PollingStation",
                column: "PollingCentreCentreId");

            migrationBuilder.CreateIndex(
                name: "IX_Vote_ElectionId",
                table: "Vote",
                column: "ElectionId");

            migrationBuilder.CreateIndex(
                name: "IX_Vote_VoterId",
                table: "Vote",
                column: "VoterId");

            migrationBuilder.CreateIndex(
                name: "IX_Voter_ConstituencyId",
                table: "Voter",
                column: "ConstituencyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Candidate_Vote");

            migrationBuilder.DropTable(
                name: "PollingStation");

            migrationBuilder.DropTable(
                name: "Vote");

            migrationBuilder.DropTable(
                name: "Candidate");

            migrationBuilder.DropTable(
                name: "PollingCentre");

            migrationBuilder.DropTable(
                name: "Election");

            migrationBuilder.DropTable(
                name: "Voter");

            migrationBuilder.DropTable(
                name: "PollingDivision");

            migrationBuilder.DropTable(
                name: "Constituency");
        }
    }
}
