using Microsoft.EntityFrameworkCore.Migrations;

namespace eVotingApi.Migrations
{
    public partial class ModifyFKPCAndPS : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PollingCentre_PollingDivision_PollingDivisionDivisionId",
                table: "PollingCentre");

            migrationBuilder.DropForeignKey(
                name: "FK_PollingStation_PollingCentre_PollingCentreCentreId",
                table: "PollingStation");

            migrationBuilder.DropIndex(
                name: "IX_PollingStation_PollingCentreCentreId",
                table: "PollingStation");

            migrationBuilder.DropIndex(
                name: "IX_PollingCentre_PollingDivisionDivisionId",
                table: "PollingCentre");

            migrationBuilder.DropColumn(
                name: "PollingCentreCentreId",
                table: "PollingStation");

            migrationBuilder.DropColumn(
                name: "PollingDivisionDivisionId",
                table: "PollingCentre");

            migrationBuilder.CreateIndex(
                name: "IX_PollingStation_CentreId",
                table: "PollingStation",
                column: "CentreId");

            migrationBuilder.CreateIndex(
                name: "IX_PollingCentre_DivisionId",
                table: "PollingCentre",
                column: "DivisionId");

            migrationBuilder.AddForeignKey(
                name: "FK_PollingCentre_PollingDivision_DivisionId",
                table: "PollingCentre",
                column: "DivisionId",
                principalTable: "PollingDivision",
                principalColumn: "DivisionId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PollingStation_PollingCentre_CentreId",
                table: "PollingStation",
                column: "CentreId",
                principalTable: "PollingCentre",
                principalColumn: "CentreId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PollingCentre_PollingDivision_DivisionId",
                table: "PollingCentre");

            migrationBuilder.DropForeignKey(
                name: "FK_PollingStation_PollingCentre_CentreId",
                table: "PollingStation");

            migrationBuilder.DropIndex(
                name: "IX_PollingStation_CentreId",
                table: "PollingStation");

            migrationBuilder.DropIndex(
                name: "IX_PollingCentre_DivisionId",
                table: "PollingCentre");

            migrationBuilder.AddColumn<long>(
                name: "PollingCentreCentreId",
                table: "PollingStation",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "PollingDivisionDivisionId",
                table: "PollingCentre",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PollingStation_PollingCentreCentreId",
                table: "PollingStation",
                column: "PollingCentreCentreId");

            migrationBuilder.CreateIndex(
                name: "IX_PollingCentre_PollingDivisionDivisionId",
                table: "PollingCentre",
                column: "PollingDivisionDivisionId");

            migrationBuilder.AddForeignKey(
                name: "FK_PollingCentre_PollingDivision_PollingDivisionDivisionId",
                table: "PollingCentre",
                column: "PollingDivisionDivisionId",
                principalTable: "PollingDivision",
                principalColumn: "DivisionId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PollingStation_PollingCentre_PollingCentreCentreId",
                table: "PollingStation",
                column: "PollingCentreCentreId",
                principalTable: "PollingCentre",
                principalColumn: "CentreId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
