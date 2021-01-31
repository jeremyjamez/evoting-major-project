using Microsoft.EntityFrameworkCore.Migrations;

namespace eVotingApi.Migrations
{
    public partial class removeRegisteredVoterCount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RegisteredVotersCount",
                table: "Constituency");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RegisteredVotersCount",
                table: "Constituency",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
