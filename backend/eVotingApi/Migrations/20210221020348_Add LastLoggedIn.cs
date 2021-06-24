using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eVotingApi.Migrations
{
    public partial class AddLastLoggedIn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastLoggedIn",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastLoggedIn",
                table: "AspNetUsers");
        }
    }
}
