using Microsoft.EntityFrameworkCore.Migrations;

namespace eVotingApi.Migrations
{
    public partial class AddParishtoVoter : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Parish",
                table: "Voter",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Parish",
                table: "Voter");
        }
    }
}
