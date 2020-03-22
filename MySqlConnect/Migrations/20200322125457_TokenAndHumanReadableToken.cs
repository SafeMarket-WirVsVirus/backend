using Microsoft.EntityFrameworkCore.Migrations;

namespace MySqlConnect.Migrations
{
    public partial class TokenAndHumanReadableToken : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HumanReadableToken",
                table: "Reservation",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReservationToken",
                table: "Reservation",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HumanReadableToken",
                table: "Reservation");

            migrationBuilder.DropColumn(
                name: "ReservationToken",
                table: "Reservation");
        }
    }
}
