using Microsoft.EntityFrameworkCore.Migrations;

namespace Homefind.Infrastructure.Migrations.EstateDb
{
    public partial class migrate_review_model : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReviewerName",
                table: "Reviews",
                nullable: true);
        }
    }
}
