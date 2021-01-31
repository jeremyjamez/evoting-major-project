using Microsoft.EntityFrameworkCore.Migrations;

namespace eVotingApi.Migrations
{
    public partial class UpdateFK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Member_Candidate_CandidateMemberId",
                table: "Member");

            migrationBuilder.DropForeignKey(
                name: "FK_Member_PoliticalParty_PoliticalPartyPartyId",
                table: "Member");

            migrationBuilder.DropIndex(
                name: "IX_Member_CandidateMemberId",
                table: "Member");

            migrationBuilder.DropIndex(
                name: "IX_Member_PoliticalPartyPartyId",
                table: "Member");

            migrationBuilder.DropColumn(
                name: "CandidateMemberId",
                table: "Member");

            migrationBuilder.DropColumn(
                name: "PoliticalPartyPartyId",
                table: "Member");

            migrationBuilder.CreateIndex(
                name: "IX_Member_PartyId",
                table: "Member",
                column: "PartyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Candidate_Member_MemberId",
                table: "Candidate",
                column: "MemberId",
                principalTable: "Member",
                principalColumn: "MemberId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Member_PoliticalParty_PartyId",
                table: "Member",
                column: "PartyId",
                principalTable: "PoliticalParty",
                principalColumn: "PartyId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Candidate_Member_MemberId",
                table: "Candidate");

            migrationBuilder.DropForeignKey(
                name: "FK_Member_PoliticalParty_PartyId",
                table: "Member");

            migrationBuilder.DropIndex(
                name: "IX_Member_PartyId",
                table: "Member");

            migrationBuilder.AddColumn<long>(
                name: "CandidateMemberId",
                table: "Member",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "PoliticalPartyPartyId",
                table: "Member",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Member_CandidateMemberId",
                table: "Member",
                column: "CandidateMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_Member_PoliticalPartyPartyId",
                table: "Member",
                column: "PoliticalPartyPartyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Member_Candidate_CandidateMemberId",
                table: "Member",
                column: "CandidateMemberId",
                principalTable: "Candidate",
                principalColumn: "MemberId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Member_PoliticalParty_PoliticalPartyPartyId",
                table: "Member",
                column: "PoliticalPartyPartyId",
                principalTable: "PoliticalParty",
                principalColumn: "PartyId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
