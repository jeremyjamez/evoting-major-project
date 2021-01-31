using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eVotingApi.Migrations
{
    public partial class Changes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Candidate_Constituency_CandidateId",
                table: "Candidate");

            migrationBuilder.DeleteData(
                table: "Election",
                keyColumn: "ElectionId",
                keyValue: 1L);

            migrationBuilder.DropColumn(
                name: "Affiliation",
                table: "Candidate");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Candidate");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Candidate");

            migrationBuilder.DropColumn(
                name: "MiddleName",
                table: "Candidate");

            migrationBuilder.DropColumn(
                name: "Parish",
                table: "Candidate");

            migrationBuilder.DropColumn(
                name: "Prefix",
                table: "Candidate");

            migrationBuilder.DropColumn(
                name: "Suffix",
                table: "Candidate");

            migrationBuilder.RenameColumn(
                name: "CandidateId",
                table: "Candidate",
                newName: "MemberId");

            migrationBuilder.AddColumn<string>(
                name: "Parish",
                table: "Constituency",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "PoliticalParty",
                columns: table => new
                {
                    PartyId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PoliticalParty", x => x.PartyId);
                });

            migrationBuilder.CreateTable(
                name: "Member",
                columns: table => new
                {
                    MemberId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Prefix = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MiddleName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Suffix = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PartyId = table.Column<long>(type: "bigint", nullable: false),
                    Position = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JoinDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PoliticalPartyPartyId = table.Column<long>(type: "bigint", nullable: true),
                    CandidateMemberId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Member", x => x.MemberId);
                    table.ForeignKey(
                        name: "FK_Member_Candidate_CandidateMemberId",
                        column: x => x.CandidateMemberId,
                        principalTable: "Candidate",
                        principalColumn: "MemberId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Member_PoliticalParty_PoliticalPartyPartyId",
                        column: x => x.PoliticalPartyPartyId,
                        principalTable: "PoliticalParty",
                        principalColumn: "PartyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Candidate_ConstituencyId",
                table: "Candidate",
                column: "ConstituencyId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Member_CandidateMemberId",
                table: "Member",
                column: "CandidateMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_Member_PoliticalPartyPartyId",
                table: "Member",
                column: "PoliticalPartyPartyId");

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

            migrationBuilder.DropTable(
                name: "Member");

            migrationBuilder.DropTable(
                name: "PoliticalParty");

            migrationBuilder.DropIndex(
                name: "IX_Candidate_ConstituencyId",
                table: "Candidate");

            migrationBuilder.DropColumn(
                name: "Parish",
                table: "Constituency");

            migrationBuilder.RenameColumn(
                name: "MemberId",
                table: "Candidate",
                newName: "CandidateId");

            migrationBuilder.AddColumn<string>(
                name: "Affiliation",
                table: "Candidate",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Candidate",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Candidate",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MiddleName",
                table: "Candidate",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Parish",
                table: "Candidate",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Prefix",
                table: "Candidate",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Suffix",
                table: "Candidate",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "Election",
                columns: new[] { "ElectionId", "ElectionDate", "ElectionType" },
                values: new object[] { 1L, new DateTime(2020, 12, 22, 11, 1, 42, 633, DateTimeKind.Local).AddTicks(892), "General" });

            migrationBuilder.AddForeignKey(
                name: "FK_Candidate_Constituency_CandidateId",
                table: "Candidate",
                column: "CandidateId",
                principalTable: "Constituency",
                principalColumn: "ConstituencyId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
