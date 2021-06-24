using Microsoft.EntityFrameworkCore.Migrations;

namespace eVotingApi.Migrations
{
    public partial class AddMoreColumnstoVoter : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FathersPlaceOfBirth",
                table: "Voter",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MothersPlaceOfBirth",
                table: "Voter",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PlaceOfBirth",
                table: "Voter",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FathersPlaceOfBirth",
                table: "Voter");

            migrationBuilder.DropColumn(
                name: "MothersPlaceOfBirth",
                table: "Voter");

            migrationBuilder.DropColumn(
                name: "PlaceOfBirth",
                table: "Voter");
        }
    }
}
