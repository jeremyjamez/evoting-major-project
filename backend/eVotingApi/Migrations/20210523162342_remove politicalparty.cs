using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eVotingApi.Migrations
{
    public partial class removepoliticalparty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Candidate_Constituency_ConstituencyId",
                table: "Candidate");

            migrationBuilder.DropForeignKey(
                name: "FK_Candidate_Election_ElectionId",
                table: "Candidate");

            migrationBuilder.DropForeignKey(
                name: "FK_Candidate_Member_CandidateId",
                table: "Candidate");

            migrationBuilder.DropForeignKey(
                name: "FK_PollingDivision_Constituency_ConstituencyId",
                table: "PollingDivision");

            migrationBuilder.DropForeignKey(
                name: "FK_Vote_Candidate_CandidateId",
                table: "Vote");

            migrationBuilder.DropForeignKey(
                name: "FK_Vote_Voter_VoterId",
                table: "Vote");

            migrationBuilder.DropForeignKey(
                name: "FK_Voter_Constituency_ConstituencyId",
                table: "Voter");

            migrationBuilder.DropTable(
                name: "Member");

            migrationBuilder.DropTable(
                name: "MemberOfParliament");

            migrationBuilder.DropTable(
                name: "PoliticalParty");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Voter",
                table: "Voter");

            migrationBuilder.DropIndex(
                name: "IX_Voter_ConstituencyId",
                table: "Voter");

            migrationBuilder.DropIndex(
                name: "IX_Vote_CandidateId",
                table: "Vote");

            migrationBuilder.DropIndex(
                name: "IX_Vote_VoterId",
                table: "Vote");

            migrationBuilder.DropIndex(
                name: "IX_PollingDivision_ConstituencyId",
                table: "PollingDivision");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Constituency",
                table: "Constituency");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Candidate",
                table: "Candidate");

            migrationBuilder.DropIndex(
                name: "IX_Candidate_ConstituencyId",
                table: "Candidate");

            migrationBuilder.DropColumn(
                name: "FathersPlaceOfBirth",
                table: "Voter");

            migrationBuilder.RenameColumn(
                name: "Role",
                table: "Voter",
                newName: "Salt");

            migrationBuilder.AlterColumn<string>(
                name: "Telephone",
                table: "Voter",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Prefix",
                table: "Voter",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "PlaceOfBirth",
                table: "Voter",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Parish",
                table: "Voter",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Occupation",
                table: "Voter",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "MothersPlaceOfBirth",
                table: "Voter",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "MothersMaidenName",
                table: "Voter",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "MiddleName",
                table: "Voter",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "Voter",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Gender",
                table: "Voter",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "Voter",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "DateOfBirth",
                table: "Voter",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "ConstituencyId",
                table: "Voter",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Voter",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "VoterId",
                table: "Voter",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "Voter",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MaritalStatus",
                table: "Voter",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isTwoFactorEnabled",
                table: "Voter",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "CandidateId1",
                table: "Vote",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VoterId1",
                table: "Vote",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ConstituencyId1",
                table: "PollingDivision",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Parish",
                table: "Constituency",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Constituency",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ConstituencyId",
                table: "Constituency",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "Constituency",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<long>(
                name: "ElectionId",
                table: "Candidate",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<string>(
                name: "ConstituencyId",
                table: "Candidate",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<string>(
                name: "CandidateId",
                table: "Candidate",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "Candidate",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Affiliation",
                table: "Candidate",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Candidate",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Candidate",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Voter",
                table: "Voter",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Constituency",
                table: "Constituency",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Candidate",
                table: "Candidate",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Vote_CandidateId1",
                table: "Vote",
                column: "CandidateId1");

            migrationBuilder.CreateIndex(
                name: "IX_Vote_VoterId1",
                table: "Vote",
                column: "VoterId1");

            migrationBuilder.CreateIndex(
                name: "IX_PollingDivision_ConstituencyId1",
                table: "PollingDivision",
                column: "ConstituencyId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Candidate_Election_ElectionId",
                table: "Candidate",
                column: "ElectionId",
                principalTable: "Election",
                principalColumn: "ElectionId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PollingDivision_Constituency_ConstituencyId1",
                table: "PollingDivision",
                column: "ConstituencyId1",
                principalTable: "Constituency",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Vote_Candidate_CandidateId1",
                table: "Vote",
                column: "CandidateId1",
                principalTable: "Candidate",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Vote_Voter_VoterId1",
                table: "Vote",
                column: "VoterId1",
                principalTable: "Voter",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Candidate_Election_ElectionId",
                table: "Candidate");

            migrationBuilder.DropForeignKey(
                name: "FK_PollingDivision_Constituency_ConstituencyId1",
                table: "PollingDivision");

            migrationBuilder.DropForeignKey(
                name: "FK_Vote_Candidate_CandidateId1",
                table: "Vote");

            migrationBuilder.DropForeignKey(
                name: "FK_Vote_Voter_VoterId1",
                table: "Vote");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Voter",
                table: "Voter");

            migrationBuilder.DropIndex(
                name: "IX_Vote_CandidateId1",
                table: "Vote");

            migrationBuilder.DropIndex(
                name: "IX_Vote_VoterId1",
                table: "Vote");

            migrationBuilder.DropIndex(
                name: "IX_PollingDivision_ConstituencyId1",
                table: "PollingDivision");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Constituency",
                table: "Constituency");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Candidate",
                table: "Candidate");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Voter");

            migrationBuilder.DropColumn(
                name: "MaritalStatus",
                table: "Voter");

            migrationBuilder.DropColumn(
                name: "isTwoFactorEnabled",
                table: "Voter");

            migrationBuilder.DropColumn(
                name: "CandidateId1",
                table: "Vote");

            migrationBuilder.DropColumn(
                name: "VoterId1",
                table: "Vote");

            migrationBuilder.DropColumn(
                name: "ConstituencyId1",
                table: "PollingDivision");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Constituency");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Candidate");

            migrationBuilder.DropColumn(
                name: "Affiliation",
                table: "Candidate");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Candidate");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Candidate");

            migrationBuilder.RenameColumn(
                name: "Salt",
                table: "Voter",
                newName: "Role");

            migrationBuilder.AlterColumn<long>(
                name: "VoterId",
                table: "Voter",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<string>(
                name: "Telephone",
                table: "Voter",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Prefix",
                table: "Voter",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PlaceOfBirth",
                table: "Voter",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Parish",
                table: "Voter",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Occupation",
                table: "Voter",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MothersPlaceOfBirth",
                table: "Voter",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MothersMaidenName",
                table: "Voter",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MiddleName",
                table: "Voter",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "Voter",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Gender",
                table: "Voter",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "Voter",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateOfBirth",
                table: "Voter",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ConstituencyId",
                table: "Voter",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Voter",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FathersPlaceOfBirth",
                table: "Voter",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Parish",
                table: "Constituency",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Constituency",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ConstituencyId",
                table: "Constituency",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<long>(
                name: "ElectionId",
                table: "Candidate",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ConstituencyId",
                table: "Candidate",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "CandidateId",
                table: "Candidate",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Voter",
                table: "Voter",
                column: "VoterId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Constituency",
                table: "Constituency",
                column: "ConstituencyId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Candidate",
                table: "Candidate",
                column: "CandidateId");

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
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MemberOfParliament_Constituency_ConstituencyId",
                        column: x => x.ConstituencyId,
                        principalTable: "Constituency",
                        principalColumn: "ConstituencyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PoliticalParty",
                columns: table => new
                {
                    PartyId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Colour = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Founded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Icon = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateofBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JoinDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MiddleName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PartyId = table.Column<long>(type: "bigint", nullable: false),
                    Photo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Position = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Prefix = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Suffix = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Telephone = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Member", x => x.MemberId);
                    table.ForeignKey(
                        name: "FK_Member_PoliticalParty_PartyId",
                        column: x => x.PartyId,
                        principalTable: "PoliticalParty",
                        principalColumn: "PartyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Voter_ConstituencyId",
                table: "Voter",
                column: "ConstituencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Vote_CandidateId",
                table: "Vote",
                column: "CandidateId");

            migrationBuilder.CreateIndex(
                name: "IX_Vote_VoterId",
                table: "Vote",
                column: "VoterId");

            migrationBuilder.CreateIndex(
                name: "IX_PollingDivision_ConstituencyId",
                table: "PollingDivision",
                column: "ConstituencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Candidate_ConstituencyId",
                table: "Candidate",
                column: "ConstituencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Member_PartyId",
                table: "Member",
                column: "PartyId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Candidate_Constituency_ConstituencyId",
                table: "Candidate",
                column: "ConstituencyId",
                principalTable: "Constituency",
                principalColumn: "ConstituencyId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Candidate_Election_ElectionId",
                table: "Candidate",
                column: "ElectionId",
                principalTable: "Election",
                principalColumn: "ElectionId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Candidate_Member_CandidateId",
                table: "Candidate",
                column: "CandidateId",
                principalTable: "Member",
                principalColumn: "MemberId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PollingDivision_Constituency_ConstituencyId",
                table: "PollingDivision",
                column: "ConstituencyId",
                principalTable: "Constituency",
                principalColumn: "ConstituencyId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Vote_Candidate_CandidateId",
                table: "Vote",
                column: "CandidateId",
                principalTable: "Candidate",
                principalColumn: "CandidateId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Vote_Voter_VoterId",
                table: "Vote",
                column: "VoterId",
                principalTable: "Voter",
                principalColumn: "VoterId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Voter_Constituency_ConstituencyId",
                table: "Voter",
                column: "ConstituencyId",
                principalTable: "Constituency",
                principalColumn: "ConstituencyId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
