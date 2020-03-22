using Microsoft.EntityFrameworkCore.Migrations;

namespace MySqlConnect.Migrations
{
    public partial class AddPlacesIdForLocation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PlacesId",
                table: "Location",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PlacesId",
                table: "Location");
        }
    }
}
